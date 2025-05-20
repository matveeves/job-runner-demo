using FluentValidation;

namespace JobRunner.Demo.Application.JobCommands;

public class TestTaskPayloadValidation : AbstractValidator<TestTaskPayload>
{
    public TestTaskPayloadValidation()
    {

        RuleFor(x => x)
            .MustAsync(IsAppExist)
            .When(x => x != null)
            .WithMessage(x => $".");

        RuleFor(x => x)
            .MustAsync(IsReportExist)
            .When(x => x != null)
            .WithMessage(x => $".");
    }

    private async Task<bool> IsAppExist(TestTaskPayload appId, CancellationToken cancellationToken = default)
    {
        return true;
    }

    private async Task<bool> IsReportExist(TestTaskPayload reportId, CancellationToken cancellationToken = default)
    {
        return true;
    }
}
