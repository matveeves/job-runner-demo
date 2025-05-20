using JobRunner.Demo.Application.Persistence.Queries;
using JobRunner.Demo.Application.SerializerSettings;
using Microsoft.Extensions.DependencyInjection;
using JobRunner.Demo.Application.Interfaces;
using JobRunner.Demo.Application.Behaviors;
using JobRunner.Demo.Application.Services;
using Newtonsoft.Json.Serialization;
using System.Reflection;
using FluentValidation;
using Newtonsoft.Json;
using MediatR;
using Mapster;

namespace JobRunner.Demo.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(o =>
                o.RegisterServicesFromAssembly(typeof(ApplicationRegistration).Assembly));

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(SetTaskStartDbStateBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationPayloadBehavior<,>))
            .AddTransient(typeof(IPipelineBehavior<,>), typeof(SetTaskFinishedDbStateBehavior<,>));

        return services;
    }

    public static IServiceCollection AddJobStarters(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<ITaskCommand>()
                .AddClasses(classes => classes.AssignableTo(typeof(JobStarter<,>)))
                    .AsSelf()
                    .WithScopedLifetime()
        );

        return services;
    }

    public static IServiceCollection AddValidator(this IServiceCollection services)
    {
        services.AddScoped<Validator>()
            .AddValidatorsFromAssemblyContaining<Validator>();

        return services;
    }

    public static IServiceCollection AddMapper(this IServiceCollection services)
    {
        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());

        return services;
    }

    public static IServiceCollection AddTaskSchedules(this IServiceCollection services)
    {
        services.AddSingleton(sp =>
        {
            var mediator = sp.GetRequiredService<IMediator>();
            //to do: написать запрос по получению задач из хранилиза
            //var taskSchedules = mediator.Send(query)
            //    .GetAwaiter().GetResult();

            //return taskSchedules;

            return new string[0];
        });

        return services;
    }

    public static IServiceCollection AddCamelCaseSettings(this IServiceCollection services)
    {
        services.AddSingleton(new CamelCaseJsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new CamelCaseNamingStrategy()
            },
            Formatting = Formatting.Indented,
            ConfigurationName = "сamelCase",
            DateFormatString = "yyyy-MM-dd HH:mm:ss.fff 'UTC'",
            DateTimeZoneHandling = DateTimeZoneHandling.Utc
        });

        return services;
    }
}
