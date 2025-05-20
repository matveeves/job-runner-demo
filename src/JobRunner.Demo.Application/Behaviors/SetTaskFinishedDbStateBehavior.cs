using JobRunner.Demo.Application.Persistence.Commands;
using JobRunner.Demo.Application.Interfaces;
using IFlow.Rsmv.Domain.Enums;
using MediatR;

namespace JobRunner.Demo.Application.Behaviors;

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

        //to do: передавать значение статуса из enum
        //await _mediator.Send(
        //    new SetTaskFinishedDbCommand(taskCommand.Id, taskCommand.EndDate,
        //    taskCommand.RetryCount),
        //    cancellationToken);

        return response; 
    }
}
