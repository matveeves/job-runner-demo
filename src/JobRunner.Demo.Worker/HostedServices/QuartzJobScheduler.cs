using JobRunner.Demo.Application.Persistence.Queries;
using JobRunner.Demo.Worker.Services;
using JobRunner.DemoIntegration.Worker.Attributes;
using MediatR;
using Quartz;
using System.Reflection;

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
            var logger = sp.GetRequiredService<ILogger<QuartzJobScheduler>>();

            var scheduler = await _schedulerFactory.GetScheduler(cancellationToken);
            var taskSchedules = await mediator.Send(
                new GetTaskScheduleDbQuery(), cancellationToken);

            foreach (var schedule in taskSchedules)
            {
                var jobType = GetJobClassType(schedule.Name);
            }

            var sdfsdfsd = taskSchedules.Select(s => quartzBuilder.BuildJob(GetJobClassType(s.Name), s))
                .ToArray();


            var sssdfsdfsd = sdfsdfsd;

        }



        //if (_validationState.Errors.Any())
        //{
        //    foreach (var error in _validationState.Errors)
        //    {
        //        _logger.LogWarning(error);
        //    }
        //}



    }
    public Task StopAsync(CancellationToken cancellationToken)
        => Task.CompletedTask;



    private Type? GetJobClassType(string jobName)
    {
        var obType = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .SingleOrDefault(t => t.GetCustomAttribute<JobNameAttribute>() != null
                && !string.IsNullOrWhiteSpace(t.GetCustomAttribute<JobNameAttribute>()!.Name)
                && t.GetCustomAttribute<JobNameAttribute>()!.Name == jobName);

        return obType;
    }
}
