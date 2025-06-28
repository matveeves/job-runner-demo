using JobRunner.Demo.Application.Persistence.Commands;
using JobRunner.Demo.Application.Interfaces;
using MediatR;

namespace JobRunner.Demo.Application.Behaviors;

/// <summary>
/// Поведение конвейера MediatR, фиксирующее запуск задачи: увеличивает счётчик попыток,
/// устанавливает дату начала и отправляет команду обновления статуса в хранилище.
/// </summary>
public class SetTaskStartDbStateBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ITaskCommand
{
    private readonly IMediator _mediator;
    public SetTaskStartDbStateBehavior(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<TResponse> Handle(TRequest taskCommand, 
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        taskCommand.TryCount++;
        taskCommand.StartDate = DateTime.UtcNow;

        await _mediator.Send(
            new SetTaskStartDbCommand(taskCommand.Id, taskCommand.StartDate),
            cancellationToken);

        return await next();
    }
}
