using Newtonsoft.Json;

namespace JobRunner.Demo.Application.SerializerSettings;

public class CamelCaseJsonSerializerSettings : JsonSerializerSettings
{
    public string? ConfigurationName { get; set; }
}
