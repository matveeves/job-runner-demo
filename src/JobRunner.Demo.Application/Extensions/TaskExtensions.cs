namespace JobRunner.Demo.Application.Extensions;

public static class TaskExtensions
{
    public static async Task<T[]> SafeWhenAll<T>(this IEnumerable<Task<T>> tasks)
    {
        try
        {
            return await Task.WhenAll(tasks);
        }
        catch
        {
            var exceptions = tasks
                .Where(t => t.IsFaulted)
                .Select(t => t.Exception!);

            if (exceptions.Any())
            {
                throw new AggregateException("One or more tasks failed.", exceptions);
            }

            throw;
        }
    }

    public static async Task SafeWhenAll(this IEnumerable<Task> tasks)
    {
        try
        {
            await Task.WhenAll(tasks);
        }
        catch
        {
            var exceptions = tasks
                .Where(t => t.IsFaulted)
                .Select(t => t.Exception!);

            if (exceptions.Any())
            {
                throw new AggregateException("One or more tasks failed.", exceptions);
            }

            throw;
        }
    }
}
