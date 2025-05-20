using JobRunner.Demo.Application.Extensions;
using MediatR;

namespace JobRunner.Demo.Application.JobCommands;

public class TestTaskHandler : IRequestHandler<TestTaskCommand, Unit>
{
    public TestTaskHandler(IServiceProvider serviceProvider)
    {
    }

    public async Task<Unit> Handle(TestTaskCommand taskCommand, CancellationToken cancellationToken)
    {
        var isTyped = taskCommand.GetTypedPayload<TestTaskPayload>(out var payload);

        await Task.Delay(35000);

        return Unit.Value;
    }
}
