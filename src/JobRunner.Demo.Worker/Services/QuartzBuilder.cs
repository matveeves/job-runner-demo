using JobRunner.Demo.Domain.Entities;
using Newtonsoft.Json;
using Quartz;

namespace JobRunner.Demo.Worker.Services;

public class QuartzBuilder
{
    public IJobDetail BuildJob(Type jobType, TaskSchedule jobSchedule)
    {
        var jobBuilder = JobBuilder.Create(jobType)
            .WithIdentity(new JobKey(jobSchedule.Name))
            .UsingJobData("taskScheduleName", jobSchedule.Name)
            .UsingJobData("maxItems", jobSchedule.MaxItemsPerIteration)
            .UsingJobData("concurrencyLimit", jobSchedule.ConcurrencyLimitPerIteration);

        var jCustomParams = !string.IsNullOrWhiteSpace(jobSchedule.JCustomParams)
            ? jobSchedule.JCustomParams
            : "{}";

        var customParams = JsonConvert
            .DeserializeObject<Dictionary<string, string>>(jCustomParams);

        foreach (var (key, value) in customParams!)
        {
            jobBuilder.UsingJobData(key, value);
        }

        return jobBuilder.Build();
    }

    public ITrigger BuildTrigger(TaskSchedule jobSchedule)
    {
        var trigger = TriggerBuilder.Create()
            .ForJob(jobSchedule.Name)
            .WithIdentity($"{jobSchedule.Name}-trigger")
            .WithCronSchedule(jobSchedule.CronExpression, c => c
                .WithMisfireHandlingInstructionDoNothing())
            .Build();

        return trigger;
    }
}
