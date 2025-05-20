namespace JobRunner.DemoIntegration.Worker.Models;

public class QuartzValidationState
{
    public ICollection<string> Errors { get; }
    public QuartzValidationState(ICollection<string> errors)
    {
        Errors = errors;
    }
}
