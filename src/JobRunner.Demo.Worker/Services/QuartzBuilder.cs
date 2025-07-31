using JobRunner.Demo.Domain.Entities;
using Newtonsoft.Json;
using Quartz;

namespace JobRunner.Demo.Worker.Services;

internal sealed class QuartzBuilder
{
    public IJobDetail BuildJob(Type jobType, TaskQueueSchedule jobQueueSchedule)
    {
        var jobBuilder = JobBuilder.Create(jobType)
            .WithIdentity(new JobKey(jobQueueSchedule.Name))
            .UsingJobData("queueScheduleName", jobQueueSchedule.Name)
            .UsingJobData("maxItems", jobQueueSchedule.MaxItemsPerIteration)
            .UsingJobData("concurrencyLimit", jobQueueSchedule.ConcurrencyLimitPerIteration);

        var jCustomParams = !string.IsNullOrWhiteSpace(jobQueueSchedule.JCustomParams)
            ? jobQueueSchedule.JCustomParams
            : "{}";

        var customParams = JsonConvert
            .DeserializeObject<Dictionary<string, string>>(jCustomParams);

        foreach (var (key, value) in customParams!)
        {
            jobBuilder.UsingJobData(key, value);
        }

        return jobBuilder.Build();
    }

    public ITrigger BuildTrigger(TaskQueueSchedule jobQueueSchedule)
    {
        var trigger = TriggerBuilder.Create()
            .ForJob(jobQueueSchedule.Name)
            .WithIdentity($"{jobQueueSchedule.Name}-trigger")
            .WithCronSchedule(jobQueueSchedule.CronExpression, c => c
                .WithMisfireHandlingInstructionDoNothing())
            .Build();

        return trigger;
    }
}
