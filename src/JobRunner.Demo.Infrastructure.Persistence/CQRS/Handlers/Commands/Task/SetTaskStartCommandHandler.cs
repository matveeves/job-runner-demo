using TaskStatus = JobRunner.Demo.Domain.Entities.TaskStatus;
using JobRunner.Demo.Application.Persistence.Commands;
using JobRunner.Demo.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using JobRunner.Demo.Domain.Enums;
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
            .Where(s => s.Code == TaskStatusCode.RUNNING)
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
