using JobRunner.Demo.Application.JobCommands;
using JobRunner.Demo.Worker.Abstractions;
using JobRunner.Demo.Worker.Attributes;

namespace JobRunner.Demo.Worker.Tasks;

[JobName("test_task")]
internal class TestJob : JobBase<TestTaskCommand, TestTaskPayload>
{
    public TestJob(IServiceScopeFactory scopeFactory)
        : base(scopeFactory)
    {
    }
}
