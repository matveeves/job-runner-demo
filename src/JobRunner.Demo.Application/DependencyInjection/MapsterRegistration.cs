using JobRunner.Demo.Application.Mapper.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using JobRunner.Demo.Application.Enums;
using System.Reflection;
using Mapster;

namespace JobRunner.Demo.Application.DI;

public static class MapsterRegistration
{
    private static readonly Dictionary<NameResolveStrategy, NameMatchingStrategy> MatchingStrategy = new()
    {
        { NameResolveStrategy.Exact, NameMatchingStrategy.Exact },
        { NameResolveStrategy.Flexible, NameMatchingStrategy.Flexible },
        { NameResolveStrategy.IgnoreCase, NameMatchingStrategy.IgnoreCase },
        { NameResolveStrategy.ToCamelCase, NameMatchingStrategy.ToCamelCase },
        { NameResolveStrategy.FromCamelCase, NameMatchingStrategy.FromCamelCase }
    };

    public static IServiceCollection AddMapper(this IServiceCollection services, IConfiguration configuration)
    {
        var mapsterSection = configuration.GetSection(MapsterOptions.SectionName);
        var mapsterOptions = mapsterSection
            .Get<MapsterOptions>() ?? new MapsterOptions();

        services.Configure<MapsterOptions>(mapsterSection);

        TypeAdapterConfig.GlobalSettings
            .Scan(Assembly.GetExecutingAssembly());

        TypeAdapterConfig.GlobalSettings.Default
            .NameMatchingStrategy(MatchingStrategy[mapsterOptions.NameResolveStrategy])
            .ShallowCopyForSameType(mapsterOptions.ShallowCopyForSameType)
            .IgnoreNullValues(mapsterOptions.IgnoreNullValues)
            .PreserveReference(mapsterOptions.PreserveReference)
            .IgnoreNonMapped(mapsterOptions.IgnoreNonMapped);

        return services;
    }
}
