using JobRunner.Demo.Application.Persistence.Commands;
using JobRunner.Demo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace JobRunner.Demo.Infrastructure.Persistence.EfCore.CQRS.Handlers.Commands;

public class SetTaskFinishedCommandHandler
    : IRequestHandler<SetTaskFinishedDbCommand, int>
{
    private AppDbContext _dbContext;
    public SetTaskFinishedCommandHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Handle(
        SetTaskFinishedDbCommand query, CancellationToken cancellationToken)
    {
        var statusToSetId = await _dbContext.Set<TaskQueueItemStatus>()
            .Where(s => s.Code == query.StatusToSetCode)
            .Select(s => s.Id)
            .SingleAsync(cancellationToken);

        return await _dbContext.Set<TaskQueueItem>()
            .Where(t => t.Id == query.TaskId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.EndDate, query.EndDate)
                .SetProperty(p => p.TryCount, query.RetryCount)
                .SetProperty(p => p.JError, query.JError)
                .SetProperty(p => p.TaskStatusId, statusToSetId),
                cancellationToken);
    }
}
