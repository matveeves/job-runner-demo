namespace JobRunner.Demo.Worker.Models;

internal class JobClassTypesContainer
{
    public IReadOnlyDictionary<string, Type> JobClassTypes { get; }
    public JobClassTypesContainer(IReadOnlyDictionary<string, Type> jobClassTypes)
    {
        JobClassTypes = jobClassTypes;
    }
}
