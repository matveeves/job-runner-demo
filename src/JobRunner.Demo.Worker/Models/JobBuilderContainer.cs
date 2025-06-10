using JobRunner.Demo.Domain.Entities;
using Quartz;

namespace JobRunner.Demo.Worker.Models;

public class JobBuilderContainer
{
    public TaskQueueSchedule QueueSchedule { get; }
    public bool IsReadyToStart { get; }
    public IJobDetail? QuartzJobDetail { get; }
    public ITrigger? QuartzJobTrigger { get; }
    public ICollection<string> ErrorMessages { get; }
    public JobBuilderContainer(bool isReadyToStart, TaskQueueSchedule queueSchedule,
        IJobDetail? quartzJobDetail, ITrigger? quartzJobTrigger, ICollection<string> errorMessages)
    {
        IsReadyToStart = isReadyToStart;
        QuartzJobDetail = quartzJobDetail;
        QuartzJobTrigger = quartzJobTrigger;
        ErrorMessages = errorMessages;
        QueueSchedule = queueSchedule;
    }
}
