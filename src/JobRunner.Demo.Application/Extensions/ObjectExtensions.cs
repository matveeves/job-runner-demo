namespace JobRunner.Demo.Application.Extensions;

public static class ObjectExtensions
{
    /// <summary>
    /// Пытается безопасно конвертировать объект в указанный примитивный тип.
    /// Возвращает false, если:
    /// - obj == null (даже для nullable-типов),
    /// - конвертация невозможна.
    /// В случае успеха возвращает true, а out-параметр содержит результат.
    /// </summary>
    public static bool TryConvert(this object obj,
        Type targetType, out object? typedObj)
    {
        typedObj = null;

        if (obj is null || obj is DBNull
            || !targetType.IsConvertSupportedType())
        {
            return false;
        }

        try
        {
            typedObj = Convert.ChangeType(obj, targetType);
            return true;
        }
        catch
        {
            return false;
        }
    }
}
