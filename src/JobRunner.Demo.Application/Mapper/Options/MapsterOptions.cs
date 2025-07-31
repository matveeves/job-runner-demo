using System.ComponentModel.DataAnnotations;
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
    [Required] public MapsterNameMatchingStrategy MapsterNameMatchingStrategy { get; set; }

    /// <summary>
    /// Включить поверхностное копирование для одинаковых типов.
    /// </summary>
    [Required] public bool ShallowCopyForSameType { get; set; }

    /// <summary>
    /// Игнорировать null свойства исходного объекта и не записывать его в целевом.
    /// </summary>
    [Required] public bool IgnoreNullValues { get; set; }

    /// <summary>
    /// Сохранять ссылочную целостность объектов.
    /// </summary>
    [Required] public bool PreserveReference { get; set; }

    /// <summary>
    /// Игнорировать свойства без явного маппинга в правилах.
    /// </summary>
    [Required] public bool IgnoreNonMapped { get; set; }
}
