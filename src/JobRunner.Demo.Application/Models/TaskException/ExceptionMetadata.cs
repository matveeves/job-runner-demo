namespace JobRunner.Demo.Application.Models;

public class ExceptionMetadata
{
    public string? ExceptionType { get; set; }
    public string Message { get; set; }
    public string? Source { get; set; }
    public InnerException? InnerException { get; set; } = new();
    public Metadata Metadata { get; set; } = new();
}
