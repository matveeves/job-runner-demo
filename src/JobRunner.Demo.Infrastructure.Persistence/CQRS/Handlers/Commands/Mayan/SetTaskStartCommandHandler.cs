using JobRunner.Demo.Application.Persistence.Commands;
using Microsoft.EntityFrameworkCore;
using IFlow.Rsmv.Domain.Entities;
using IFlow.Rsmv.Domain.Enums;
using IFlow.Rsmv.DataAccess;
using MediatR;

namespace JobRunner.Demo.Infrastructure.Persistence.CQRS.Handlers.Commands;

public class SetTaskStartCommandHandler
    : IRequestHandler<SetTaskStartDbCommand, int>
{
    private AppDbContext _dbContext;
    public SetTaskStartCommandHandler(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<int> Handle(
        SetTaskStartDbCommand query, CancellationToken cancellationToken)
    {
        var runningStatusId = await _dbContext.Set<TaskStatus>()
            .Where(s => s.Code == .RUNNING)
            .Select(s => s.Id)
            .SingleAsync(cancellationToken);

        return await _dbContext.Set<TaskQueue>()
            .Where(t => t.Id == query.TaskId)
            .ExecuteUpdateAsync(s => s
                .SetProperty(p => p.StartDate, query.StartDate)
                .SetProperty(p => p.TaskStatusId, runningStatusId),
                cancellationToken);
    }
}
