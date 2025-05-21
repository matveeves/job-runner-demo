using Serilog;

namespace JobRunner.DemoIntegration.Worker.DependencyInjection;

public static class WorkerRegistration
{
    public static IServiceCollection AddWorker(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(configuration)
            .AddSerilog(o => o.ReadFrom.Configuration(configuration));

        return services;
    }
}
