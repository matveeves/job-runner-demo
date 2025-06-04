using JobRunner.Demo.Infrastructure.Persistence.EfCore;

namespace JobRunner.DemoIntegration.Worker.DependencyInjection;

public static class InfrastructureRegistration
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddEfCore(configuration);

        return services;
    }
}
