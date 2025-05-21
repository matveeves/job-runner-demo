using JobRunner.Demo.Application.Persistence.Queries;
using Microsoft.EntityFrameworkCore;
using JobRunner.Demo.Domain.Entities;
using JobRunner.Demo.Domain.Enums;
using MediatR;

namespace JobRunner.Demo.Infrastructure.Persistence.EfCore.CQRS.Handlers;

public class GetTaskQueueByScheduleQueryHandler
    : IRequestHandler<GetTaskQueueByScheduleDbQuery, IReadOnlyCollection<TaskQueue>>
{
    private AppDbContext _dbContext;
    public GetTaskQueueByScheduleQueryHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<IReadOnlyCollection<TaskQueue>> Handle(
        GetTaskQueueByScheduleDbQuery query, CancellationToken cancellationToken)
    {
        //to do: передать лимит (Take(query.Limit))
        var statuses = new[] { TaskStatusCode.PENDING, TaskStatusCode.RETRYING };

        var queue = await _dbContext.Set<TaskQueue>()
            .Include(q => q.TaskSchedule)
            .AsNoTracking()
            .Where(q => q.TaskSchedule!.Name == query.ScheduleName
                && statuses.Contains(q.TaskStatus!.Code)
                && (!q.StartByDate.HasValue || q.StartByDate > DateTime.UtcNow)
                && q.RetryCount < q.TaskSchedule!.MaxRetries
                && !q.IsManual)
            .ToArrayAsync(cancellationToken);

        return queue;
    }
}
