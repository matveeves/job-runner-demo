using JobRunner.Demo.Application.Interfaces;
using MediatR;

namespace JobRunner.Demo.Application.Abstractions;

public abstract class TaskCommandBase : IRequest<Unit>, ITaskCommand
{
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int TryCount { get; set; }
    public int MaxTries { get; set; }
    public string? ExceptionsJson { get; set; }
    public ITaskPayload Payload { get; set; }
}
