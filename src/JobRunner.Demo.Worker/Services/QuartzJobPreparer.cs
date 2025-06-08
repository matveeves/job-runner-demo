using JobRunner.Demo.Application.Persistence.Queries;
using JobRunner.Demo.Domain.Entities;
using JobRunner.Demo.Worker.Models;
using MediatR;

namespace JobRunner.Demo.Worker.Services;

public class QuartzJobPreparer
{
    public async Task<ICollection<QuartzJobBuilderContainer>> PrepareQuartzJobItems(
        IServiceProvider serviceProvider, CancellationToken cancellationToken = default)
    {
        var mediator = serviceProvider.GetRequiredService<IMediator>();
        var quartzBuilder = serviceProvider.GetRequiredService<QuartzBuilder>();
        var jobScheduleValidator = serviceProvider.GetRequiredService<JobScheduleValidator>();
        var jobClassTypes = serviceProvider.GetRequiredService<JobClassTypesContainer>();

        var jobSchedules = await mediator.Send(
            new GetTaskScheduleDbQuery(), cancellationToken);

        return jobSchedules.Select(s
                => BuildJobContainer(s, quartzBuilder, jobScheduleValidator, jobClassTypes))
            .ToArray();
    }

    private static QuartzJobBuilderContainer BuildJobContainer(TaskSchedule schedule, QuartzBuilder quartzBuilder,
        JobScheduleValidator scheduleValidator, JobClassTypesContainer jobClassTypes)
    {
        var jobType = jobClassTypes.JobClassTypes[schedule.Name];
        var isReadyToStart = scheduleValidator.Validate(
            schedule, jobType, out var errorMessages);

        var jobDetail = isReadyToStart
            ? quartzBuilder.BuildJob(jobType!, schedule)
            : null;

        var jobTrigger = isReadyToStart
            ? quartzBuilder.BuildTrigger(schedule)
            : null;

        return new QuartzJobBuilderContainer(isReadyToStart, schedule,
            jobDetail, jobTrigger, errorMessages);
    }
}
