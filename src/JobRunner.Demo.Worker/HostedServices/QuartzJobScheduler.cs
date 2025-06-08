            using JobRunner.Demo.Application.Persistence.Queries;
using JobRunner.DemoIntegration.Worker.Attributes;
using JobRunner.Demo.Worker.Services;
using System.Reflection;
using MediatR;
using Quartz;
using JobRunner.Demo.Domain.Entities;
using JobRunner.Demo.Worker.Models;

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
            var sp = scope.ServiceProvider;
            var mediator = sp.GetRequiredService<IMediator>();
            var quartzBuilder = sp.GetRequiredService<QuartzBuilder>();
            var scheduleValidator = sp.GetRequiredService<JobScheduleValidator>();
            var logger = sp.GetRequiredService<ILogger<QuartzJobScheduler>>();
            var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            var taskSchedules = await mediator.Send(
                new GetTaskScheduleDbQuery(), cancellationToken);

            if (taskSchedules.Count == 0)
            {
                logger.LogWarning($"В конфигурации не обнаружены " +
                        $"зарегистрированные задачи. Запуск Quartz будет пропущен.");
                return;
            }

            var jobsToStart = taskSchedules.Select(s
                    => BuildJobContainer(s, quartzBuilder, scheduleValidator))
                .Where(s => s.IsReadyToStart)
                .ToArray();

            await scheduler.Start(cancellationToken);
            _ = jobsToStart.Select(async c 
                    => await scheduler.ScheduleJob(c.QuartzJobDetail!, c.QuartzJobTrigger!, cancellationToken))
                .ToArray();


            if (scheduleValidator.Errors.Any())
            {
                foreach (var error in scheduleValidator.Errors)
                {
                    logger.LogWarning(error);
                }
            }
        }
    }
    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;

    private static QuartzJobBuilderContainer BuildJobContainer(TaskSchedule schedule,
        QuartzBuilder quartzBuilder, JobScheduleValidator validator)
    {
        var jobType = GetJobClassType(schedule.Name);
        var isReadyToStart = validator.Validate(schedule, jobType);

        var jobDetail = isReadyToStart
            ? quartzBuilder.BuildJob(jobType!, schedule)
            : null;

        var jobTrigger = isReadyToStart
            ? quartzBuilder.BuildTrigger(schedule)
            : null;

        return new QuartzJobBuilderContainer(isReadyToStart, schedule, jobDetail, jobTrigger);
    }

    private static Type? GetJobClassType(string jobName)
    {
        var jobType = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .SingleOrDefault(t => t.GetCustomAttribute<JobNameAttribute>() != null
                && !string.IsNullOrWhiteSpace(t.GetCustomAttribute<JobNameAttribute>()!.Name)
                && t.GetCustomAttribute<JobNameAttribute>()!.Name == jobName);

        return jobType;
    }
}
