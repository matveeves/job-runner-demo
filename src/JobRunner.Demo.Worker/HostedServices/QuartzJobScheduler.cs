using JobRunner.Demo.Worker.Services;
using JobRunner.Demo.Worker.Models;
using Quartz;

namespace JobRunner.Demo.Worker.HostedServices;

public class QuartzJobScheduler : IHostedService
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly IServiceScopeFactory _scopeFactory;
    public QuartzJobScheduler(ISchedulerFactory schedulerFactory,
        IServiceScopeFactory scopeFactory)
    {
        _schedulerFactory = schedulerFactory;
        _scopeFactory = scopeFactory;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using (var scope = _scopeFactory.CreateAsyncScope())
        {
            var serviceProvider = scope.ServiceProvider;
            var quartzJobPreparer = serviceProvider.GetRequiredService<QuartzJobPreparer>();
            var logger = serviceProvider.GetRequiredService<ILogger<QuartzJobScheduler>>();

            var quartzScheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            await quartzScheduler.Start(cancellationToken);

            var jobContainers = 
                await quartzJobPreparer.PrepareQuartzJobItems(cancellationToken);

            if (jobContainers.Count == 0)
            {
                logger.LogWarning($"В конфигурации не обнаружены " +
                        $"зарегистрированные задачи. Сервис не будет запускать задачи.");
                return;
            }

            var jobScheduleTasks = jobContainers
                .Where(c => c.IsReadyToStart)
                .Select(c
                    => quartzScheduler.ScheduleJob(c.QuartzJobDetail!, c.QuartzJobTrigger!, cancellationToken))
                .ToArray();

            await Task.WhenAll(jobScheduleTasks);

            LogErrorJobs(jobContainers, logger);
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    private void LogErrorJobs(ICollection<JobBuilderContainer> jobContainers,
        ILogger<QuartzJobScheduler> logger)
    {
        
        var jobContainerErrors = jobContainers
            .Where(c => !c.IsReadyToStart)
            .SelectMany(c => c.ErrorMessages)
            .ToArray();

        foreach (var error in jobContainerErrors)
        {
            logger.LogWarning(error);
        }
    }
}
