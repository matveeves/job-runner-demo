using JobRunner.DemoIntegration.Worker.DependencyInjection;
using JobRunner.Demo.Application;

namespace JobRunner.DemoIntegration.Worker;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = Host.CreateApplicationBuilder(args);

        builder.Configuration
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddEnvironmentVariables();

        builder.Services.AddApplication()
            .AddInfrastructure(builder.Configuration)
            .AddWorker(builder.Configuration);

        var host = builder.Build();

        host.Run();
    }
}
