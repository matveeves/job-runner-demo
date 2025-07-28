using JobRunner.Demo.Worker.Attributes;
using JobRunner.Demo.Worker.Services;
using JobRunner.Demo.Worker.Models;
using System.Reflection;
using Serilog;

namespace JobRunner.Demo.Worker.DependencyInjection;

public static class WorkerRegistration
{
    public static IServiceCollection AddWorker(
        this IServiceCollection services, IConfiguration configuration)
    {
        services.AddQuartz(configuration)
            .AddSerilog(o => o.ReadFrom.Configuration(configuration));

        services.AddSingleton(_ =>
        {
            var jobClassTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a => a.GetTypes())
                .Where(t => t.GetCustomAttribute<JobNameAttribute>() != null
                            && !string.IsNullOrWhiteSpace(t.GetCustomAttribute<JobNameAttribute>()!.Name))
                .ToDictionary(key => key.GetCustomAttribute<JobNameAttribute>()!.Name, value => value);

            return new JobClassTypesContainer(jobClassTypes);
        });

        return services;
    }
}
