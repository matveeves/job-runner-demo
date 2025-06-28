namespace JobRunner.Demo.Application.Interfaces;

public interface ITaskCommand
{
    Guid Id { get; set; }
    DateTime StartDate { get; set; }
    DateTime EndDate { get; set; }
    int TryCount { get; set; }
    int MaxTries { get; set; }
    string? ExceptionsJson { get; set; }
    ITaskPayload Payload { get; set; }
}
