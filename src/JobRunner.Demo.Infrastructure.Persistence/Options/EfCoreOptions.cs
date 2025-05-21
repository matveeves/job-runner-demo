using System.ComponentModel.DataAnnotations;

namespace JobRunner.Demo.Infrastructure.Persistence.Options;

public class EfCoreOptions
{
    public static string SectionName = "EfCore";

    [Required]
    public string DbServer { get; set; }

    [Required]
    public string DbPort { get; set; }

    [Required]
    public string DbName { get; set; }

    [Required]
    public string DbUserName { get; set; }

    [Required]
    public string DbPassword { get; set; }
    public string ConnectionString
    {
        get => $"Server={DbServer};Port={DbPort};Database={DbName};Username={DbUserName};Password={DbPassword}";
    }
}
