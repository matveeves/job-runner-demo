using JobRunner.Demo.Application.Persistence.Queries;
using JobRunner.Demo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace JobRunner.Demo.Infrastructure.Persistence.EfCore.CQRS.Handlers;

public class GetTaskScheduleQueryHandler
    : IRequestHandler<GetTaskScheduleDbQuery, IReadOnlyCollection<TaskQueueSchedule>>
{
    private AppDbContext _dbContext;
    public GetTaskScheduleQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<TaskQueueSchedule>> Handle(
        GetTaskScheduleDbQuery query, CancellationToken cancellationToken)
    {
        var schedules = await _dbContext
            .Set<TaskQueueSchedule>().AsNoTracking()
            .Where(s => s.IsEnabled)
            .ToArrayAsync(cancellationToken);

        return schedules;
    }
}
