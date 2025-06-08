using JobRunner.DemoIntegration.Worker.Extensions;
using JobRunner.Demo.Domain.Entities;
using Quartz;

namespace JobRunner.Demo.Worker.Services;

public class JobScheduleValidator
{
    public ICollection<string> Errors { get; }
    public JobScheduleValidator()
    {
        Errors = new List<string>();
    }

    public bool Validate(TaskSchedule jobSchedule, Type? jobType)
    {
        if (jobType == null)
        {
            Errors.Add($"Не удаётся определить класс-обработчик для задачи '{jobSchedule.Name}'. " +
                       $"Запуск задачи '{jobSchedule.Name}' будет пропущен.");
        }

        if (!CronExpression.IsValidExpression(jobSchedule.CronExpression))
        {
            Errors.Add($"Обнаружено невалидное cron выражение для задачи '{jobSchedule.Name}'. " +
                       $"Запуск задачи '{jobSchedule.Name}' будет пропущен.");
        }

        var jCustomParams = jobSchedule.JCustomParams;
        if (jCustomParams != null && !jCustomParams.IsValidJson())
        {
            Errors.Add($"Невалидный json кастомных параметров для задачи '{jobSchedule.Name}'. " +
                       $"Запуск задачи '{jobSchedule.Name}' будет пропущен.");
        }

        return jobType != null
               && CronExpression.IsValidExpression(jobSchedule.CronExpression)
               && (jCustomParams == null || jCustomParams.IsValidJson());
    }
}
