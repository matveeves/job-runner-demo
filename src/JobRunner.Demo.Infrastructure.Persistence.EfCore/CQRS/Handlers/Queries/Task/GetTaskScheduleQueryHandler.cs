using JobRunner.Demo.Application.Persistence.Queries;
using JobRunner.Demo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace JobRunner.Demo.Infrastructure.Persistence.EfCore.CQRS.Handlers;

public class GetTaskScheduleQueryHandler
    : IRequestHandler<GetTaskScheduleDbQuery, IReadOnlyCollection<TaskSchedule>>
{
    private AppDbContext _dbContext;
    public GetTaskScheduleQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<TaskSchedule>> Handle(
        GetTaskScheduleDbQuery query, CancellationToken cancellationToken)
    {
        var schedules = await _dbContext
            .Set<TaskSchedule>().AsNoTracking()
            .Where(s => s.IsEnabled)
            .ToArrayAsync(cancellationToken);

        return schedules;
    }
}
