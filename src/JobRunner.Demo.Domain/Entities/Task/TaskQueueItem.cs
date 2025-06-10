using JobRunner.Demo.Domain.Abstractions;

namespace JobRunner.Demo.Domain.Entities;

/// <summary>
/// Задача из очереди
/// </summary>
public class TaskQueueItem : AuditableEntity, IBaseEntity<Guid>
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Параметры запуска в формате JSON
    /// </summary>
    public string JPayload { get; set; } = null!;

    /// <summary>
    /// Информация об ошибке в формате JSON
    /// </summary>
    public string? JError { get; set; }

    /// <summary>
    /// Запуск по времени (дата и время запуска)
    /// </summary>
    public DateTime? StartByDate { get; set; }

    /// <summary>
    /// Дата запуска
    /// </summary>
    public DateTime? StartDate { get; set; }

    /// <summary>
    /// Дата завершения
    /// </summary>
    public DateTime? EndDate { get; set; }

    /// <summary>
    /// Количество попыток перезапуска задачи
    /// </summary>
    public int RetryCount { get; set; } = 0;

    /// <summary>
    /// Признак ручного запуска
    /// </summary>
    public bool IsManual { get; set; }

    /// <summary>
    /// Идентификатор конфигурации
    /// </summary>
    public Guid TaskScheduleId { get; set; }

    /// <summary>
    /// Идентификатор статуса
    /// </summary>
    public Guid TaskStatusId { get; set; }

    /// <summary>
    /// Конфигурация
    /// </summary>
    public virtual TaskQueueSchedule? TaskSchedule { get; set; }

    /// <summary>
    /// Статус
    /// </summary>
    public virtual TaskQueueItemStatus? TaskStatus { get; set; }
}
