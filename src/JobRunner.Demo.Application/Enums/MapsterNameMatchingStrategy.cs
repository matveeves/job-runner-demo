namespace JobRunner.Demo.Application.Enums;

/// <summary>
/// Стратегии сопоставления имён свойств при маппинге с использованием Mapster.
/// </summary>
public enum MapsterNameMatchingStrategy : byte
{
    /// <summary>
    /// Только точное совпадение (регистрозависимое).
    /// </summary>
    Exact,

    /// <summary>
    /// Допускает различия (пробелы, подчёркивания, регистр).
    /// </summary>
    Flexible,

    /// <summary>
    /// Точное совпадение, но без учёта регистра.
    /// </summary>
    IgnoreCase,

    /// <summary>
    /// Сопоставляет имена, преобразуя их в camelCase.
    /// </summary>
    ToCamelCase,

    /// <summary>
    /// Сопоставляет имена, преобразуя из camelCase.
    /// </summary>
    FromCamelCase
}
