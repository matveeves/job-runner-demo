namespace JobRunner.Demo.Domain.Enums;

/// <summary>
/// Статус выполнения задачи.
/// </summary>
public enum TaskStatusCode
{
    /// <summary>
    /// Задача находится в очереди на выполнение.
    /// </summary>
    PENDING,

    /// <summary>
    /// Задача в процессе выполнения.
    /// </summary>
    RUNNING,

    /// <summary>
    /// Задача успешно завершена.
    /// </summary>
    SUCCESS,

    /// <summary>
    /// Выполнение задачи завершилось с ошибкой.
    /// </summary>
    FAILED,

    /// <summary>
    /// Задача повторно запускается после ошибки.
    /// </summary>
    RETRYING,

    /// <summary>
    /// Задача была отменена.
    /// </summary>
    CANCELLED
}
