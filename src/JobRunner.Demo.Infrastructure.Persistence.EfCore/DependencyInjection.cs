using JobRunner.Demo.Infrastructure.Persistence.EfCore.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace JobRunner.Demo.Infrastructure.Persistence.EfCore;

public static class DependencyInjection
{
    public static IServiceCollection AddEfCore(
        this IServiceCollection services, IConfiguration configuration)
    {
        var sectionName = EfCoreOptions.SectionName;

        services.AddEfCore(o =>
        {
            o.DbServer = "localhost";
            o.DbPort = "5432";
            o.DbName = "jobs";
            o.DbUserName = "postgres";
            o.DbPassword = "postgres";
        }).AddMediatR(o =>
            o.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));
        //services.AddEfCore(o =>
        //{
        //    o.DbServer = configuration[$"{sectionName}:DbServer"]!;
        //    o.DbPort = configuration[$"{sectionName}:DbPort"]!;
        //    o.DbName = configuration[$"{sectionName}:DbName"]!;
        //    o.DbUserName = configuration[$"{sectionName}:DbUserName"]!;
        //    o.DbPassword = configuration[$"{sectionName}:DbPassword"]!;
        //}).AddMediatR(o =>
        //    o.RegisterServicesFromAssembly(typeof(DependencyInjection).Assembly));

        return services;
    }

    public static IServiceCollection AddEfCore(this IServiceCollection services, Action<EfCoreOptions>? setupAction = null)
    {
        if (setupAction != null)
            services.ConfigureEfCore(setupAction);

        services.AddDbContext<AppDbContext>((sp, o) =>
        {
            var efCoreOptions = sp.GetService<IOptions<EfCoreOptions>>()?.Value;

            if (efCoreOptions == null)
                throw new ArgumentNullException($"The section '{EfCoreOptions.SectionName}' " +
                    $"is missing from the application configuration");

            o.UseNpgsql(efCoreOptions.ConnectionString, builder =>
            {
                builder.MigrationsAssembly(typeof(AppDbContext).Assembly.FullName);
                builder.MigrationsHistoryTable("cs_ef_migrations", "jobs");
            });
            //o.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        return services;
    }
    public static void ConfigureEfCore(this IServiceCollection services,
        Action<EfCoreOptions> setupAction)
    {
        services.AddOptions<EfCoreOptions>()
            .ValidateDataAnnotations()
            .ValidateOnStart()
            .Configure(setupAction);
    }
}
