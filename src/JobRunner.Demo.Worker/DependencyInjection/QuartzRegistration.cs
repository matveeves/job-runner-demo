using JobRunner.Demo.Worker.HostedServices;
using JobRunner.Demo.Worker.Services;
using Quartz;

namespace JobRunner.Demo.Worker.DependencyInjection;

internal static class QuartzRegistration
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

        services.AddHostedService<QuartzJobScheduleHostedService>()
            .AddSingleton<JobScheduleValidator>()
            .AddSingleton<QuartzBuilder>()
            .AddScoped<QuartzJobPreparer>()
            .AddScoped<QuartzJobScheduler>();

        return services;
    }
}
