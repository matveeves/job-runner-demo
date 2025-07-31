using JobRunner.Demo.Application.Interfaces;
using JobRunner.Demo.Application.Services;
using Quartz;

namespace JobRunner.Demo.Worker.Abstractions;

[DisallowConcurrentExecution]
internal abstract class JobBase<TCommand, TPayload> : IJob
    where TCommand : ITaskCommand
    where TPayload : ITaskPayload
{
    private readonly IServiceScopeFactory _scopeFactory;
    public JobBase(IServiceScopeFactory scopeFactory)
    {
        _scopeFactory = scopeFactory;
    }
    public async Task Execute(IJobExecutionContext context)
    {
        await using var scope = _scopeFactory.CreateAsyncScope();

        var jobData = context.JobDetail.JobDataMap;
        var jobName = jobData["queueScheduleName"];
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<JobBase<TCommand, TPayload>>>();

        try
        {
            context.CancellationToken.ThrowIfCancellationRequested();
            logger.LogInformation($"Job '{jobName}' started.");

            await DoJob(jobData, scope.ServiceProvider, context.CancellationToken);

            logger.LogInformation($"Job '{jobName}' finished successfully.");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"Job '{jobName}' failed.");
        }
    }

    protected virtual async Task DoJob(JobDataMap jobData, IServiceProvider serviceProvider,
        CancellationToken cancellationToken = default)
    {
        var maxItems = jobData.GetInt("maxItems");
        var concurrencyLimit = jobData.GetInt("concurrencyLimit");
        var taskScheduleName = jobData.GetString("queueScheduleName")!;
        var jobStarter = serviceProvider.GetRequiredService<JobStarter<TCommand, TPayload>>();

        var defaultTaskDataKeys = new[] {
            "queueScheduleName", "maxItems", "concurrencyLimit" };
        var customJobData = jobData
            .Where(data => !defaultTaskDataKeys.Contains(data.Key))
            .ToDictionary(data => data.Key, data => data.Value);
        
        await jobStarter.WithMaxItems(maxItems)
            .WithConcurrencyLimit(concurrencyLimit)
            .WithCustomCommandParams(customJobData)
            .StartJob(taskScheduleName, cancellationToken);
    }
}
