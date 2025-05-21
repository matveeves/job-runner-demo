using JobRunner.Demo.Domain.Enums;
using MediatR;

namespace JobRunner.Demo.Application.Persistence.Commands;

public class SetTaskFinishedDbCommand : IRequest<int>
{
    public Guid TaskId { get; }
    public DateTime EndDate { get; }
    public int RetryCount { get; }
    public string? JError { get; }
    public TaskStatusCode StatusToSetCode { get; }
    public SetTaskFinishedDbCommand(Guid taskId, DateTime endDate,
        int retryCount, TaskStatusCode statusToSetCode, string? jError = null)
    {
        TaskId = taskId;
        JError = jError;
        EndDate = endDate;
        RetryCount = retryCount;
        StatusToSetCode = statusToSetCode;
    }
}
