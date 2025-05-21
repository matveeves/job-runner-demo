using JobRunner.Demo.Domain.Abstractions;
using JobRunner.Demo.Domain.Enums;

namespace JobRunner.Demo.Domain.Entities;

/// <summary>
/// Статус задачи
/// </summary>
public class TaskStatus : AuditableEntity, IBaseEntity<Guid>
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
    /// Код
    /// </summary>
    public TaskStatusCode Code { get; set; }

    public virtual ICollection<TaskQueue> Tasks { get; set; } = new List<TaskQueue>();
}
