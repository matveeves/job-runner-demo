using JobRunner.DemoIntegration.Worker.DependencyInjection;
using JobRunner.Demo.Infrastructure.Persistence.EfCore;
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

        builder.Services.AddApplication();
        builder.Services.ConfigureEfCore(builder.Configuration);
        builder.Services.AddQuartz(builder.Configuration);

        var host = builder.Build();

        host.Run();
    }
}