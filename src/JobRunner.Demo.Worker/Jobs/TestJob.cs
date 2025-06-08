using JobRunner.Demo.Worker.Abstractions;
using JobRunner.Demo.Worker.Attributes;
using JobRunner.Demo.Application.JobCommands;

namespace JobRunner.Demo.Worker.Tasks;

[JobName("test_task")]
public class TestJob : JobBase<TestTaskCommand, TestTaskPayload>
{
    public TestJob(IServiceScopeFactory scopeFactory)
        : base(scopeFactory)
    {
    }
}
