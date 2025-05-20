using JobRunner.DemoIntegration.Worker.Models;

namespace JobRunner.DemoIntegration.Worker.Services;

public class QuartzValidationService : IHostedService
{
    private readonly QuartzValidationState _validationState;
    private readonly ILogger<QuartzValidationService> _logger;
    public QuartzValidationService(
        QuartzValidationState validationState,
        ILogger<QuartzValidationService> logger)
    {
        _validationState = validationState;
        _logger = logger;
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        if (_validationState.Errors.Any())
        {
            foreach (var error in _validationState.Errors)
            {
                _logger.LogWarning(error);
            }
        }

        return Task.CompletedTask;
    }
    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}
