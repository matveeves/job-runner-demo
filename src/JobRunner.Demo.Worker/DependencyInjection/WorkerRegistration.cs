using JobRunner.Demo.Worker.HostedServices;
using JobRunner.Demo.Worker.Services;
using Serilog;

namespace JobRunner.DemoIntegration.Worker.DependencyInjection;

public static class WorkerRegistration
{
    public static IServiceCollection AddWorker(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(configuration)
            .AddSingleton<QuartzBuilder>()
            .AddHostedService<QuartzJobScheduler>()
            .AddSerilog(o => o.ReadFrom.Configuration(configuration));

        return services;
    }
}
