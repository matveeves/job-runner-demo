using JobRunner.Demo.Application.Interfaces;

namespace JobRunner.Demo.Application.Extensions;

public static class TaskCommandExtensions
{
    public static bool GetTypedPayload<TPayload>(this ITaskCommand command, out TPayload? typedPayload)
        where TPayload : class, ITaskPayload
    {
        if (command.Payload is TPayload payload)
        {
            typedPayload = payload;
            return true;
        }

        typedPayload = null;
        return false;
    }
}
