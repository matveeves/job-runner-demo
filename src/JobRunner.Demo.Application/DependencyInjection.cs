using JobRunner.Demo.Application.SerializerSettings;
using Microsoft.Extensions.DependencyInjection;
using JobRunner.Demo.Application.Interfaces;
using JobRunner.Demo.Application.Behaviors;
using JobRunner.Demo.Application.Services;
using Newtonsoft.Json.Serialization;
using FluentValidation;
using Newtonsoft.Json;
using MediatR;

namespace JobRunner.Demo.Application;

public static class DependencyInjection
{
    /// <summary>
    /// Регистрирует MediatR и конвейер поведения для обработки задач.
    /// Конвейер содержит обработку исключений, логирование, валидацию и обновление состояния задачи в хранилище.
    /// Конвейер выполняется в порядке регистрации.
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
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
