using JobRunner.Demo.Worker.Extensions;
using JobRunner.Demo.Domain.Entities;
using Quartz;

namespace JobRunner.Demo.Worker.Services;

internal sealed class JobScheduleValidator
{
    public bool Validate(TaskQueueSchedule jobQueueSchedule, Type? jobType, out ICollection<string> errorMessages)
    {
        errorMessages = new List<string>();

        if (jobType == null)
        {
            errorMessages.Add($"Не удаётся определить класс-обработчик для задачи '{jobQueueSchedule.Name}'. " +
                              $"Запуск задачи '{jobQueueSchedule.Name}' будет пропущен.");
        }

        if (!CronExpression.IsValidExpression(jobQueueSchedule.CronExpression))
        {
            errorMessages.Add($"Обнаружено невалидное cron выражение для задачи '{jobQueueSchedule.Name}'. " +
                              $"Запуск задачи '{jobQueueSchedule.Name}' будет пропущен.");
        }

        var jCustomParams = jobQueueSchedule.JCustomParams;
        if (jCustomParams != null && !jCustomParams.IsValidJson())
        {
            errorMessages.Add($"Невалидный json кастомных параметров для задачи '{jobQueueSchedule.Name}'. " +
                              $"Запуск задачи '{jobQueueSchedule.Name}' будет пропущен.");
        }

        return jobType != null
               && CronExpression.IsValidExpression(jobQueueSchedule.CronExpression)
               && (jCustomParams == null || jCustomParams.IsValidJson());
    }
}
