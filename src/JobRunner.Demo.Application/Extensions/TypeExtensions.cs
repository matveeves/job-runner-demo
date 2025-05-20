namespace JobRunner.Demo.Application.Extensions;

public static class TypeExtensions
{
    public static bool IsConvertSupportedType(this Type type)
    {
        return type.IsPrimitive ||
               type == typeof(string) ||
               type == typeof(decimal) ||
               type == typeof(DateTime) ||
               type == typeof(Guid) ||
               type == typeof(TimeSpan);
    }
}
