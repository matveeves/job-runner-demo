namespace JobRunner.Demo.Application.Models;

public class TaskException
{
    public TaskMetadata Task { get; set; } = new();
    public ExceptionMetadata Exception { get; set; } = new();
}
