using JobRunner.Demo.Worker.Extensions;
using JobRunner.Demo.Domain.Entities;
using Quartz;

namespace JobRunner.Demo.Worker.Services;

public class JobScheduleValidator
{
    public bool Validate(TaskSchedule jobSchedule, Type? jobType, out ICollection<string> errorMessages)
    {
        errorMessages = new List<string>();

        if (jobType == null)
        {
            errorMessages.Add($"Не удаётся определить класс-обработчик для задачи '{jobSchedule.Name}'. " +
                              $"Запуск задачи '{jobSchedule.Name}' будет пропущен.");
        }

        if (!CronExpression.IsValidExpression(jobSchedule.CronExpression))
        {
            errorMessages.Add($"Обнаружено невалидное cron выражение для задачи '{jobSchedule.Name}'. " +
                              $"Запуск задачи '{jobSchedule.Name}' будет пропущен.");
        }

        var jCustomParams = jobSchedule.JCustomParams;
        if (jCustomParams != null && !jCustomParams.IsValidJson())
        {
            errorMessages.Add($"Невалидный json кастомных параметров для задачи '{jobSchedule.Name}'. " +
                              $"Запуск задачи '{jobSchedule.Name}' будет пропущен.");
        }

        return jobType != null
               && CronExpression.IsValidExpression(jobSchedule.CronExpression)
               && (jCustomParams == null || jCustomParams.IsValidJson());
    }
}
