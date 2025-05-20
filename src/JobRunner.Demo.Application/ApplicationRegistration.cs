using Microsoft.Extensions.DependencyInjection;

namespace JobRunner.Demo.Application;

public static class ApplicationRegistration
{
    public static IServiceCollection AddApplication(
        this IServiceCollection services)
    {
        services.AddCamelCaseSettings()
            .AddTaskSchedules()
            .AddJobStarters()
            .AddValidator()
            .AddMediator()
            .AddMapper();

        return services;
    }
}
