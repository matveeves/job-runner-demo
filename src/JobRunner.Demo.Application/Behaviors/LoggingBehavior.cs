using JobRunner.Demo.Application.Interfaces;
using Microsoft.Extensions.Logging;
using MediatR;

namespace JobRunner.Demo.Application.Behaviors;

/// <summary>
/// Поведение конвейера MediatR, логирующее началои успешное завершение выполнения задачи.
/// </summary>
public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ITaskCommand
{
    private readonly ILogger<TRequest> _logger;
    public LoggingBehavior(ILogger<TRequest> logger)
    {
        _logger = logger;
    }

    public async Task<TResponse> Handle(TRequest taskCommand, 
        RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Task execution started. TaskId: '{TaskId}', " +
                               "StartTime: '{StartTime:dd.MM.yyyy HH:mm:ss}', RetryCount: '{RetryCount}/{MaxRetries}' .",
            taskCommand.Id, taskCommand.StartDate.ToLocalTime(), taskCommand.RetryCount, taskCommand.MaxRetries);

        var response = await next();

        _logger.LogInformation("Task execution completed successfully. TaskId: '{TaskId}', " +
                               "EndTime: '{EndTime:dd.MM.yyyy HH:mm:ss}', RetryCount: '{RetryCount}/{MaxRetries}'.",
            taskCommand.Id, taskCommand.EndDate.ToLocalTime(), taskCommand.RetryCount, taskCommand.MaxRetries);

        return response;
    }
}
