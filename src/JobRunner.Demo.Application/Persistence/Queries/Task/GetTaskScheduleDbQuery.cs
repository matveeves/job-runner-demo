using JobRunner.Demo.Domain.Entities;
using MediatR;

namespace JobRunner.Demo.Application.Persistence.Queries;

public class GetTaskScheduleDbQuery : IRequest<IReadOnlyCollection<TaskQueueSchedule>>
{
    public bool IsEnabled { get; set; }
    public GetTaskScheduleDbQuery(bool isEnabled = true)
    {
        IsEnabled = isEnabled;
    }
}
