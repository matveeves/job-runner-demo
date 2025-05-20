using JobRunner.Demo.Application.Interfaces;

namespace JobRunner.Demo.Application.JobCommands;

public class TestTaskPayload : ITaskPayload
{
    public DocumentType DocumentType { get; set; }
    public IReadOnlyCollection<MetadataType> MetadataTypes { get; set; }
}

public class DocumentType
{
    public string Label { get; set; }
}

public class MetadataType
{
    public string Name { get; set; }
    public string Label { get; set; }
    public string? Lookup { get; set; }
    public string? Default { get; set; }
}
