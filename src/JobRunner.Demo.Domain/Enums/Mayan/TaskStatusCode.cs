namespace JobRunner.Demo.Domain.Enums;

/// <summary>
/// Статус выполнения задачи.
/// </summary>
public enum TaskStatusCode
{
    /// <summary>
    /// Задача находится в очереди на выполнение.
    /// </summary>
    Pending,

    /// <summary>
    /// Задача в процессе выполнения.
    /// </summary>
    Running,

    /// <summary>
    /// Задача успешно завершена.
    /// </summary>
    Success,

    /// <summary>
    /// Выполнение задачи завершилось с ошибкой.
    /// </summary>
    Failed,

    /// <summary>
    /// Задача повторно запускается после ошибки.
    /// </summary>
    Retrying,

    /// <summary>
    /// Задача была отменена.
    /// </summary>
    Cancelled
}
