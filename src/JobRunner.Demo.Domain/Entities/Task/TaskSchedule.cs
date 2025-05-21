using JobRunner.Demo.Domain.Abstractions;

namespace JobRunner.Demo.Domain.Entities;

/// <summary>
/// Конфигурация задач
/// </summary>
public class TaskSchedule : AuditableEntity, IBaseEntity<Guid>
{
    /// <summary>
    /// Идентификатор
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Название
    /// </summary>
    public string Name { get; set; } = null!;

    /// <summary>
    /// Описание
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Cron-выражение
    /// </summary>
    public string CronExpression { get; set; } = null!;

    /// <summary>
    /// Признак активности
    /// </summary>
    public bool IsEnabled { get; set; } = false;

    /// <summary>
    /// Количество задач, обрабатываемых за один проход
    /// </summary>
    public int MaxItemsPerIteration { get; set; } = 50;

    /// <summary>
    /// Количество параллельных задач, обрабатываемых за один проход
    /// </summary>
    public int ConcurrencyLimitPerIteration { get; set; } = 20;

    /// <summary>
    /// Дополнительные параметры в формате JSON
    /// </summary>
    public string? JCustomParams { get; set; }

    /// <summary>
    /// Количество попыток повторного запуска задачи
    /// </summary>
    public int MaxRetries { get; set; } = 3;

    public virtual ICollection<TaskQueue> Tasks { get; set; } = new List<TaskQueue>();
}
