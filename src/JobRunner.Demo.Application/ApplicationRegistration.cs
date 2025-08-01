using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using JobRunner.Demo.Application.DI;

namespace JobRunner.Demo.Application;

public static class ApplicationRegistration
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddCamelCaseSettings()
            .AddMapper(configuration)
            .AddJobStarters()
            .AddValidator()
            .AddMediator();

        return services;
    }
}
