namespace JobRunner.Demo.Application.Models;

public class TaskMetadata
{
    public Guid Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int RetryCount { get; set; }
    public int MaxRetries { get; set; }
}
