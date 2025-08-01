using JobRunner.Demo.Application.Enums;

namespace JobRunner.Demo.Application.Mapper.Options;

/// <summary>
/// Конфигурацию маппера 'Mapster'.
/// </summary>
public class MapsterOptions
{
    public static string SectionName = "Mapster";

    /// <summary>
    /// Стратегия сопоставления имён свойств.
    /// </summary>
    public NameResolveStrategy NameResolveStrategy { get; set; } = NameResolveStrategy.Flexible;

    /// <summary>
    /// Включить поверхностное копирование для одинаковых типов.
    /// </summary>
    public bool ShallowCopyForSameType { get; set; } = true;

    /// <summary>
    /// Игнорировать null свойства исходного объекта и не записывать его в целевом.
    /// </summary>
    public bool IgnoreNullValues { get; set; } = true;

    /// <summary>
    /// Сохранять ссылочную целостность объектов.
    /// </summary>
    public bool PreserveReference { get; set; } = true;

    /// <summary>
    /// Игнорировать свойства без явного маппинга в правилах.
    /// </summary>
    public bool IgnoreNonMapped { get; set; } = false;
}
