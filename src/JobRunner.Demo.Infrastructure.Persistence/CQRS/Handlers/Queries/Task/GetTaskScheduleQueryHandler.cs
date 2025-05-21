using JobRunner.Demo.Infrastructure.Persistence.Extensions;
using JobRunner.Demo.Infrastructure.Persistence.Options;
using JobRunner.Demo.Application.Persistence.Queries;
using JobRunner.Demo.Domain.Entities;
using Microsoft.Extensions.Options;
using MediatR;
using Npgsql;

namespace JobRunner.Demo.Infrastructure.Persistence.CQRS.Handlers;

/// <summary>
/// Обработчик на получение расписания запуска задач.
/// Прямое использование Npgsql по той причине, что запрос отправляется
/// при запуске приложения во время регистрации зависимостей DI.
/// </summary>
public class GetTaskScheduleQueryHandler
    : IRequestHandler<GetTaskScheduleDbQuery, IReadOnlyCollection<TaskSchedule>>
{
    private EfCoreOptions _options;
    public GetTaskScheduleQueryHandler(IOptions<EfCoreOptions> options)
    {
        _options = options.Value;
    }

    public async Task<IReadOnlyCollection<TaskSchedule>> Handle(
        GetTaskScheduleDbQuery query, CancellationToken cancellationToken)
    {
        using var connection = new NpgsqlConnection(_options.ConnectionString);
        await connection.OpenAsync(cancellationToken);
        var command = new NpgsqlCommand("SELECT * FROM jobs.cs_task_schedules WHERE b_is_enabled;", connection);
        using var reader = await command.ExecuteReaderAsync(cancellationToken);

        var schedules = new List<TaskSchedule>();
        while (await reader.ReadAsync())
        {
            schedules.Add(new TaskSchedule
            {
                Id = reader.GetGuid(0),
                Name = reader.GetString(1),
                Description = reader.GetNullableString(2),
                CronExpression = reader.GetString(3),
                IsEnabled = reader.GetBoolean(4),
                MaxItemsPerIteration = reader.GetInt32(5),
                JCustomParams = reader.GetNullableString(6),
                ConcurrencyLimitPerIteration = reader.GetInt32(7),
                Creator = reader.GetNullableString(8),
                CreateDate = reader.GetNullableDateTime(9),
                Owner = reader.GetNullableString(10),
                ModifDate = reader.GetNullableDateTime(11),
                MaxRetries = reader.GetInt32(12)
            });
        }

        return schedules.ToArray();
    }
}
