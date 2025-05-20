using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using IFlow.Rsmv.DataAccess.Options;
using IFlow.Rsmv.DataAccess;

namespace JobRunner.Demo.Infrastructure.Persistence;

public static class DependencyInjection
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services, IConfiguration configuration)
    {
        var sectionName = EfCoreOptions.SectionName;
        services.AddEfCore(o =>
        {
            o.DbServer = configuration[$"{sectionName}:DbServer"]!;
            o.DbPort = configuration[$"{sectionName}:DbPort"]!;
            o.DbName = configuration[$"{sectionName}:DbName"]!;
            o.DbUserName = configuration[$"{sectionName}:DbUserName"]!;
            o.DbPassword = configuration[$"{sectionName}:DbPassword"]!;
        }).AddMediatR(o =>
            o.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }
}
