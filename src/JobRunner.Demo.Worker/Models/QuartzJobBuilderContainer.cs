using JobRunner.Demo.Domain.Entities;
using Quartz;

namespace JobRunner.Demo.Worker.Models;

public class QuartzJobBuilderContainer
{
    public TaskSchedule Schedule { get; }
    public bool IsReadyToStart { get; }
    public IJobDetail? QuartzJobDetail { get; }
    public ITrigger? QuartzJobTrigger { get; }
    public QuartzJobBuilderContainer(bool isReadyToStart, TaskSchedule schedule,
        IJobDetail? quartzJobDetail, ITrigger? quartzJobTrigger)
    {
        IsReadyToStart = isReadyToStart;
        QuartzJobDetail = quartzJobDetail;
        QuartzJobTrigger = quartzJobTrigger;
        Schedule = schedule;
    }
}
