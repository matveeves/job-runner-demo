using Npgsql;

namespace JobRunner.Demo.Infrastructure.Persistence.Extensions;

public static class NpgsqlDataReaderExtensions
{
    public static string? GetNullableString(this NpgsqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? null : reader.GetString(ordinal);

    public static DateTime? GetNullableDateTime(this NpgsqlDataReader reader, int ordinal) =>
        reader.IsDBNull(ordinal) ? null : reader.GetDateTime(ordinal);
}
