using JobRunner.Demo.Application.Persistence.Commands;
using JobRunner.Demo.Application.SerializerSettings;
using JobRunner.Demo.Application.Interfaces;
using JobRunner.Demo.Application.Models;
using Microsoft.Extensions.Logging;
using JobRunner.Demo.Domain.Enums;
using Newtonsoft.Json;
using Mapster;
using MediatR;

namespace JobRunner.Demo.Application.Behaviors;

public class ExceptionHandlingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ITaskCommand
{
    private readonly IMediator _mediator;
    private readonly ILogger<TRequest> _logger;
    private readonly CamelCaseJsonSerializerSettings _serializerSettings;
    public ExceptionHandlingBehavior(ILogger<TRequest> logger, CamelCaseJsonSerializerSettings serializerSettings, IMediator mediator)
    {
        _logger = logger;
        _serializerSettings = serializerSettings;
        _mediator = mediator;
    }

    public async Task<TResponse> Handle(TRequest taskCommand, 
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        
        try
        {
            return await next();
        }
        catch (Exception ex)
        {
            await OnThrowException(taskCommand, ex, cancellationToken);

            _logger.LogError(ex, "An exception occurred during task execution. " +
                "TaskId: '{TaskId}', EndDate: '{EndDate:dd.MM.yyyy HH:mm:ss}', RetryCount: '{RetryCount}/{MaxRetries}'",
                taskCommand.Id, taskCommand.EndDate.ToLocalTime(), taskCommand.RetryCount, taskCommand.MaxRetries);

            throw;
        }
    }

    private async Task OnThrowException(ITaskCommand taskCommand,
        Exception exception, CancellationToken cancellationToken = default)
    {
        taskCommand.EndDate = DateTime.UtcNow;

        var exceptions = JsonConvert.DeserializeObject<ICollection<TaskException>>
            (taskCommand.ExceptionsJson ?? "[]", _serializerSettings)!;

        var taskException = taskCommand.Adapt<TaskException>();
        taskException = exception.Adapt(taskException);
        exceptions.Add(taskException);

        var exceptionsJson = JsonConvert.SerializeObject(exceptions, _serializerSettings);
        var statusToSetCode = taskCommand.RetryCount == taskCommand.MaxRetries
            ? TaskStatusCode.FAILED
            : TaskStatusCode.RETRYING;

        await _mediator.Send(
            new SetTaskFinishedDbCommand(taskCommand.Id, taskCommand.EndDate,
                taskCommand.RetryCount, statusToSetCode, exceptionsJson),
        cancellationToken);
    }
}
