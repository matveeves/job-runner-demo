using JobRunner.Demo.Application.Persistence.Queries;
using JobRunner.Demo.Domain.Entities;
using JobRunner.Demo.Worker.Models;
using MediatR;

namespace JobRunner.Demo.Worker.Services;

public class QuartzJobPreparer
{
    private readonly IMediator _mediator;
    private readonly QuartzBuilder _quartzBuilder;
    private readonly JobScheduleValidator _scheduleValidator;
    private readonly JobClassTypesContainer _jobClassTypes;
    public QuartzJobPreparer(IMediator mediator, QuartzBuilder quartzBuilder,
        JobScheduleValidator jobScheduleValidator, JobClassTypesContainer jobClassTypes)
    {
        _mediator = mediator;
        _quartzBuilder = quartzBuilder;
        _scheduleValidator = jobScheduleValidator;
        _jobClassTypes = jobClassTypes;
    }

    public async Task<ICollection<JobBuilderContainer>> PrepareQuartzJobItems(CancellationToken cancellationToken = default)
    {
        var jobSchedules = await _mediator.Send(
            new GetTaskScheduleDbQuery(), cancellationToken);

        return jobSchedules.Select(BuildJobContainer)
            .ToArray();
    }

    private JobBuilderContainer BuildJobContainer(TaskSchedule schedule)
    {
        _ = _jobClassTypes.JobClassTypes.TryGetValue(schedule.Name, out var jobType);
        var isReadyToStart = _scheduleValidator.Validate(
            schedule, jobType, out var errorMessages);

        var jobDetail = isReadyToStart
            ? _quartzBuilder.BuildJob(jobType!, schedule)
            : null;

        var jobTrigger = isReadyToStart
            ? _quartzBuilder.BuildTrigger(schedule)
            : null;

        return new JobBuilderContainer(isReadyToStart, schedule,
            jobDetail, jobTrigger, errorMessages);
    }
}
