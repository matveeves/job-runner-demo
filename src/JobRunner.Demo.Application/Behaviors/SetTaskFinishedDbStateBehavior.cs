using JobRunner.Demo.Application.Persistence.Commands;
using JobRunner.Demo.Application.Interfaces;
using JobRunner.Demo.Domain.Enums;
using MediatR;

namespace JobRunner.Demo.Application.Behaviors;

/// <summary>
/// Поведение конвейера MediatR, фиксирующее успешное завершение задачи:
/// устанавливает дату окончания, номер попытки выполнения и отправляет команду обновления статуса в хранилище.
/// </summary>
public class SetTaskFinishedDbStateBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ITaskCommand
{
    private readonly IMediator _mediator;
    public SetTaskFinishedDbStateBehavior(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<TResponse> Handle(TRequest taskCommand, 
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var response = await next();

        taskCommand.EndDate = DateTime.UtcNow;

        await _mediator.Send(
            new SetTaskFinishedDbCommand(taskCommand.Id, taskCommand.EndDate,
            taskCommand.RetryCount, TaskStatusCode.SUCCESS),
            cancellationToken);

        return response; 
    }
}
