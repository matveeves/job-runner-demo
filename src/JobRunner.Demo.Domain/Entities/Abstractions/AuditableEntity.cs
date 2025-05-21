namespace JobRunner.Demo.Domain.Abstractions;

public abstract class AuditableEntity
{
    public string? Creator { get; set; }
    public DateTime? CreateDate { get; set; }
    public string? Owner { get; set; }
    public DateTime? ModifDate { get; set; }
}
