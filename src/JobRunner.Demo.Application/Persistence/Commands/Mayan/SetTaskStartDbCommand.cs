using MediatR;

namespace JobRunner.Demo.Application.Persistence.Commands;

public class SetTaskStartDbCommand : IRequest<int>
{
    public Guid TaskId { get; }
    public DateTime StartDate { get; }
    public SetTaskStartDbCommand(Guid taskId, DateTime startDate)
    {
        TaskId = taskId;
        StartDate = startDate;
    }
}
