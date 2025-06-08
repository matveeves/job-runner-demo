namespace JobRunner.Demo.Worker.Models;

public class JobClassTypesContainer
{
    public IReadOnlyDictionary<string, Type> JobClassTypes { get; }
    public JobClassTypesContainer(IReadOnlyDictionary<string, Type> jobClassTypes)
    {
        JobClassTypes = jobClassTypes;
    }
}
