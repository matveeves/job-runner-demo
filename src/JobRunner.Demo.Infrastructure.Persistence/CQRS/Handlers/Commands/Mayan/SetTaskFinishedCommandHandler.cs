using JobRunner.Demo.Application.Persistence.Commands;
using Microsoft.EntityFrameworkCore;
using IFlow.Rsmv.Domain.Entities;
using IFlow.Rsmv.DataAccess;
using MediatR;

namespace JobRunner.Demo.Infrastructure.Persistence.CQRS.Handlers.Commands;

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
        var statusToSetId = await _dbContext.Set<TaskStatus>()
            .Where(s => s.Code == query.StatusToSetCode)
            .Select(s => s.Id)
            .SingleAsync(cancellationToken);

        return await _dbContext.Set<TaskQueue>()
            .Where(t => t.Id == query.TaskId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.EndDate, query.EndDate)
                .SetProperty(p => p.RetryCount, query.RetryCount)
                .SetProperty(p => p.JError, query.JError)
                .SetProperty(p => p.TaskStatusId, statusToSetId),
                cancellationToken);
    }
}
