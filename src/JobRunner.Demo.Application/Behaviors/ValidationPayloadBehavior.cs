using JobRunner.Demo.Application.Interfaces;
using JobRunner.Demo.Application.Services;
using MediatR;


namespace JobRunner.Demo.Application.Behaviors;

/// <summary>
/// Поведение конвейера MediatR, выполняющее валидацию полезной нагрузки задачи перед её обработкой.
/// При обнаружении ошибок выбрасывает исключение и прерывает выполнение.
/// </summary>
public class ValidationPayloadBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ITaskCommand
{
    private readonly IMediator _mediator;
    private readonly Validator _validator;
    public ValidationPayloadBehavior(IMediator mediator, Validator validator)
    {
        _mediator = mediator;
        _validator = validator;
    }

    public async Task<TResponse> Handle(TRequest taskCommand, 
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        await _validator.ValidateOrThrowAsync(taskCommand.Payload!);

        return await next();
    }
}
