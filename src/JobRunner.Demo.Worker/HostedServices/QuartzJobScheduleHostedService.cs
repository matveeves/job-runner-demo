using JobRunner.Demo.Worker.Services;

namespace JobRunner.Demo.Worker.HostedServices;

public class QuartzJobScheduleHostedService : IHostedService
{
    private readonly IServiceScopeFactory _scopeFactory;
    public QuartzJobScheduleHostedService(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using (var scope = _scopeFactory.CreateAsyncScope())
        {
            var serviceProvider = scope.ServiceProvider;
            var quartzJobScheduler = serviceProvider.GetRequiredService<QuartzJobScheduler>();

            await quartzJobScheduler.ScheduleJobs(cancellationToken);
        }
    }
    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;
}
