using JobRunner.Demo.Domain.Entities;
using Quartz;

namespace JobRunner.Demo.Worker.Models;

public class JobBuilderContainer
{
    public TaskSchedule Schedule { get; }
    public bool IsReadyToStart { get; }
    public IJobDetail? QuartzJobDetail { get; }
    public ITrigger? QuartzJobTrigger { get; }
    public ICollection<string> ErrorMessages { get; }
    public JobBuilderContainer(bool isReadyToStart, TaskSchedule schedule,
        IJobDetail? quartzJobDetail, ITrigger? quartzJobTrigger, ICollection<string> errorMessages)
    {
        IsReadyToStart = isReadyToStart;
        QuartzJobDetail = quartzJobDetail;
        QuartzJobTrigger = quartzJobTrigger;
        ErrorMessages = errorMessages;
        Schedule = schedule;
    }
}
