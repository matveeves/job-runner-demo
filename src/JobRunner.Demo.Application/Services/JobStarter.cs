using JobRunner.Demo.Application.Persistence.Queries;
using Microsoft.Extensions.DependencyInjection;
using JobRunner.Demo.Application.Extensions;
using JobRunner.Demo.Application.Interfaces;
using System.Reflection;
using Newtonsoft.Json;
using Mapster;
using MediatR;

namespace JobRunner.Demo.Application.Services;

public class JobStarter<TCommand, TPayload>
    where TCommand : ITaskCommand
    where TPayload : ITaskPayload
{
    private readonly IMediator _mediator;
    private readonly IServiceScopeFactory _scopeFactory;
    private int MaxItems { get; set; } = 50;
    private int ConcurrencyLimit { get; set; } = 10;
    private Dictionary<string, object>? TaskCommandParams { get; set; }

    public JobStarter(IMediator mediator, IServiceScopeFactory scopeFactory)
    {
        _mediator = mediator;
        _scopeFactory = scopeFactory;
    }

    public JobStarter<TCommand, TPayload> WithMaxItems(int maxItems)
    {
        MaxItems = maxItems;
        return this;
    }

    public JobStarter<TCommand, TPayload> WithConcurrencyLimit(int concurrencyLimit)
    {
        ConcurrencyLimit = concurrencyLimit;
        return this;
    }

    public JobStarter<TCommand, TPayload> WithCustomCommandParams(Dictionary<string, object> taskCommandParams)
    {
        TaskCommandParams = taskCommandParams;
        return this;
    }

    public async Task StartJob(string taskScheduleName, CancellationToken cancellationToken = default)
    {
        ValidateJobBeforeStart(taskScheduleName);
        await StartJobInternal(taskScheduleName);
    }

    /// <summary>
    /// Оркестратор выполнения задач одного типа (расписания).
    /// Загружает очередь задач и запускает каждую в отдельном scope,
    /// чтобы изолировать зависимости (например, DbContext) и избежать
    /// проблем при параллельной обработке.
    /// 
    /// Задачи исполняются асинхронно с ограничением по уровню параллелизма.
    /// </summary>
    /// <param name="taskScheduleName">Имя расписания, по которому выбираются задачи</param>
    /// <param name="cancellationToken">Токен отмены для остановки выполнения</param>
    private async Task StartJobInternal(string taskScheduleName, CancellationToken cancellationToken = default)
    {
        var semaphore = new SemaphoreSlim(ConcurrencyLimit);
        var getQueueQuery = new GetTaskQueueByScheduleDbQuery(taskScheduleName, MaxItems);
        var queue = await _mediator.Send(getQueueQuery, cancellationToken);

        var tasks = queue.Select(async q =>
        {
            await semaphore.WaitAsync(cancellationToken);

            try
            {
                await using var scope = _scopeFactory.CreateAsyncScope();
                var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
                var payloadJson = !string.IsNullOrWhiteSpace(q.JPayload) ? q.JPayload : "{}";

                var taskCommand = TaskCommandParams != null && TaskCommandParams.Count > 0
                    ? CreateTaskCommand(scope.ServiceProvider, TaskCommandParams)
                    : CreateTaskCommand(scope.ServiceProvider);

                taskCommand.Payload = JsonConvert.DeserializeObject<TPayload>(payloadJson)
                    ?? throw new InvalidOperationException($"Failed to deserialize json " +
                    $"'{payloadJson}' to type '{typeof(TPayload).Name}'");

                q.Adapt(taskCommand);

                await mediator.Send(taskCommand, cancellationToken);
            }
            catch
            {
                throw;
            }
            finally
            {
                semaphore.Release();
            }
        }).ToArray();

        await tasks.SafeWhenAll();
    }

    private ITaskCommand CreateTaskCommand(IServiceProvider serviceProvider,
        Dictionary<string, object> commandParams)
    {
        var activatorConstructor = typeof(TCommand).GetConstructors()
            .Single(c => c.GetCustomAttributes(typeof(ActivatorUtilitiesConstructorAttribute)).Any());

        var parameters = activatorConstructor.GetParameters()
            .Where(p => p.Name != null)
            .Select(p => ValidateCommandParams(p, commandParams))
            .ToArray();

        var taskCommandObj = ActivatorUtilities.CreateInstance(serviceProvider, typeof(TCommand), parameters);

        if (!(taskCommandObj is ITaskCommand taskCommand))
            //написать информативное сообщение об ошибке
            throw new InvalidCastException();

        return taskCommand;
    }

    private ITaskCommand CreateTaskCommand(IServiceProvider serviceProvider)
    {
        var taskCommandObj = ActivatorUtilities.CreateInstance(serviceProvider, typeof(TCommand));

        if (!(taskCommandObj is ITaskCommand taskCommand))
            //написать информативное сообщение об ошибке
            throw new InvalidCastException();

        return taskCommand;
    }

    private object? ValidateCommandParams(ParameterInfo parameter, Dictionary<string, object> commandParams)
    {
        if (!commandParams.ContainsKey(parameter.Name!))
            //написать информативное сообщение об ошибке
            throw new ArgumentException($"Required parametr '{parameter.Name}' not found");

        var convertResult = commandParams[parameter.Name!]
            .TryConvert(parameter.ParameterType, out var converedObj);

        if (!convertResult)
        {
            //написать информативное сообщение об ошибке
            throw new InvalidCastException($"Invalid cast '{parameter.Name}' to '{parameter.ParameterType}'");
        }

        return converedObj;
    }

    private void ValidateJobBeforeStart(string taskScheduleName)
    {
        if (string.IsNullOrWhiteSpace(taskScheduleName))
            throw new ArgumentNullException(nameof(taskScheduleName));
    }
}
