using JobRunner.DemoIntegration.Worker.Abstractions;
using JobRunner.DemoIntegration.Worker.Attributes;
using JobRunner.Demo.Application.JobCommands;

namespace JobRunner.DemoIntegration.Worker.Tasks;

[JobName("test_task")]
public class TestJob : JobBase<TestTaskCommand, TestTaskPayload>
{
    public TestJob(IServiceScopeFactory scopeFactory)
        : base(scopeFactory)
    {
    }
}
