using JobRunner.Demo.Domain.Entities;
using MediatR;

namespace JobRunner.Demo.Application.Persistence.Queries;

public class GetTaskQueueByScheduleDbQuery : IRequest<IReadOnlyCollection<TaskQueue>>
{
    public string ScheduleName { get; set; }
    public int Limit { get; set; }
    public GetTaskQueueByScheduleDbQuery(string scheduleName, int limit)
    {
        ScheduleName = scheduleName;
        Limit = limit;
    }
}
