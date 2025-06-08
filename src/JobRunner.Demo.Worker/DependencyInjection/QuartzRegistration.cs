using JobRunner.Demo.Worker.HostedServices;
using JobRunner.Demo.Worker.Services;
using Quartz;

namespace JobRunner.Demo.Worker.DependencyInjection;

public static class QuartzRegistration
{
    public static IServiceCollection AddQuartz(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddQuartz(q =>
        {
            //to do: передавать из конфигурации
            q.UseDefaultThreadPool(tp =>
            {
                tp.MaxConcurrency = 75;
            });
        });

        services.AddHostedService<QuartzJobScheduler>()
            .AddSingleton<JobScheduleValidator>();

        return services;
    }
}
