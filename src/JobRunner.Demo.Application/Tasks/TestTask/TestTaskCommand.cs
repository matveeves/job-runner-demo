using Microsoft.Extensions.DependencyInjection;
using JobRunner.Demo.Application.Abstractions;

namespace JobRunner.Demo.Application.JobCommands;

public class TestTaskCommand : TaskCommandBase
{
    public string Property1 { get; }
    public int Property2 { get; }
    public bool Property3 { get; }

    [ActivatorUtilitiesConstructor]
    public TestTaskCommand(string property1, int property2, bool property3)
    {
        Property1 = property1;
        Property2 = property2;
        Property3 = property3;
    }

    public TestTaskCommand(bool property3)
    {
        Property3 = property3;
    }
}
