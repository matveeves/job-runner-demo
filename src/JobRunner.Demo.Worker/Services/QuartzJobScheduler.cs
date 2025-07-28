using JobRunner.Demo.Worker.Models;
using Quartz;

namespace JobRunner.Demo.Worker.Services;

internal sealed class QuartzJobScheduler
{
    private readonly ISchedulerFactory _schedulerFactory;
    private readonly ILogger<QuartzJobScheduler> _logger;
    private readonly QuartzJobPreparer _quartzJobPreparer;
    public QuartzJobScheduler(ISchedulerFactory schedulerFactory,
        ILogger<QuartzJobScheduler> logger,
        QuartzJobPreparer quartzJobPreparer)
    {
        _schedulerFactory = schedulerFactory;
        _quartzJobPreparer = quartzJobPreparer;
        _logger = logger;
    }
    public async Task ScheduleJobs(CancellationToken cancellationToken = default)
    {
        var quartzScheduler = await _schedulerFactory.GetScheduler(cancellationToken);
        await quartzScheduler.Start(cancellationToken);

        var jobContainers =
            await _quartzJobPreparer.PrepareQuartzJobItems(cancellationToken);

        if (jobContainers.Count == 0)
        {
            _logger.LogWarning("В конфигурации не обнаружены " +
                               "зарегистрированные расписания задач. Сервис не будет выполнять задачи.");
            return;
        }

        var jobScheduleTasks = GetJobScheduleTasks(
            jobContainers, quartzScheduler, cancellationToken);

        await Task.WhenAll(jobScheduleTasks);

        var jobContainerErrors = GetJobContainerErrors(jobContainers);
        foreach (var error in jobContainerErrors)
        {
            _logger.LogWarning(error);
        }
    }
    private Task<DateTimeOffset>[] GetJobScheduleTasks(IReadOnlyCollection<JobBuilderContainer> jobContainers,
        IScheduler quartzScheduler, CancellationToken cancellationToken = default)
        => jobContainers
            .Where(c => c.IsReadyToStart)
            .Select(c
                => quartzScheduler.ScheduleJob(c.QuartzJobDetail!, c.QuartzJobTrigger!, cancellationToken))
            .ToArray();
    private string[] GetJobContainerErrors(IReadOnlyCollection<JobBuilderContainer> jobContainers)
        => jobContainers
            .Where(c => !c.IsReadyToStart)
            .SelectMany(c => c.ErrorMessages)
            .ToArray();
}
