using JobRunner.DemoIntegration.Worker.Attributes;
using JobRunner.DemoIntegration.Worker.Extensions;
using JobRunner.DemoIntegration.Worker.Services;
using JobRunner.DemoIntegration.Worker.Models;
using IFlow.Rsmv.Domain.Entities;
using System.Reflection;
using Newtonsoft.Json;
using Quartz;

namespace JobRunner.DemoIntegration.Worker.DependencyInjection;

/// <summary>
/// Регистрация jobs в quartz.
/// Так как конфигурация задач хранится в бд, собираем di-контейнер при регистрации.
/// </summary>
public static class QuartzRegistration
{
    public static IServiceCollection AddQuartz(this IServiceCollection services,
        IConfiguration configuration)
    {
        using var sp = services.BuildServiceProvider();
        var jobSchedules = sp.GetRequiredService<IReadOnlyCollection<TaskSchedule>>();
        var jobRegisterErrors = new List<string>();

        if (jobSchedules.Any())
        {
            services.AddQuartz(q =>
            {
                foreach (var jobSchedule in jobSchedules)
                {
                    var jobType = GetJobClassType(jobSchedule.Name);

                    if(!ValidateJobSchedule(jobSchedule, jobType, jobRegisterErrors))
                    {
                        continue;
                    }

                    q.Addjob(jobType!, jobSchedule);
                }

                services.AddQuartzHostedService(o =>
                {
                    o.WaitForJobsToComplete = true;
                });
            });
        }
        else
        {
            jobRegisterErrors.Add($"В конфигурации не обнаружены " +
                $"зарегистрированные задачи. Запуск Quartz будет пропущен.");
        }

        services.AddSingleton(new QuartzValidationState(jobRegisterErrors));
        services.AddHostedService<QuartzValidationService>();

        return services;
    }


    private static Type? GetJobClassType(string jobName)
    {
        var obType = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .SingleOrDefault(t => t.GetCustomAttribute<JobNameAttribute>() != null
                                  && !string.IsNullOrWhiteSpace(t.GetCustomAttribute<JobNameAttribute>()!.Name)
                                  && t.GetCustomAttribute<JobNameAttribute>()!.Name == jobName);

        return obType;
    }

    public static void Addjob(this IServiceCollectionQuartzConfigurator configurator,
        Type jobType, TaskSchedule jobSchedule)
    {
        configurator.AddJob(jobType, new JobKey(jobSchedule.Name), j =>
        {
            j.UsingJobData("taskScheduleName", jobSchedule.Name);
            j.UsingJobData("maxItems", jobSchedule.MaxItemsPerIteration);
            j.UsingJobData("concurrencyLimit", jobSchedule.ConcurrencyLimitPerIteration);

            if (!string.IsNullOrWhiteSpace(jobSchedule.JCustomParams))
            {
                var customParams = JsonConvert
                    .DeserializeObject<Dictionary<string, string>>(jobSchedule.JCustomParams);

                foreach (var (key, value) in customParams!)
                {
                    j.UsingJobData(key, value);
                }
            }
        });

        configurator.AddTrigger(o => o
            .ForJob(jobSchedule.Name)
            .WithIdentity($"{jobSchedule.Name}-trigger")
            .WithCronSchedule(jobSchedule.CronExpression, c => c
                .WithMisfireHandlingInstructionDoNothing()));
    }

    private static bool ValidateJobSchedule(TaskSchedule jobSchedule, Type? jobType, ICollection<string> jobRegisterErrors)
    {

        if (jobType == null)
        {
            jobRegisterErrors.Add($"Не удаётся определить класс-обработчик для задачи '{jobSchedule.Name}'. " +
                          $"Запуск задачи '{jobSchedule.Name}' будет пропущен.");

        }

        if (!CronExpression.IsValidExpression(jobSchedule.CronExpression))
        {
            jobRegisterErrors.Add($"Обнаружено невалидное cron выражение для задачи '{jobSchedule.Name}'. " +
                          $"Запуск задачи '{jobSchedule.Name}' будет пропущен.");
        }

        var jCustomParams = jobSchedule.JCustomParams;

        if (jCustomParams != null && !jCustomParams.IsValidJson())
        {
            jobRegisterErrors.Add($"Невалидный json кастомных параметров для задачи '{jobSchedule.Name}'. " +
                          $"Запуск задачи '{jobSchedule.Name}' будет пропущен.");
        }

        return jobRegisterErrors.Count == 0;
    }
}
