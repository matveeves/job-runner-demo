namespace JobRunner.Demo.Domain.Abstractions;

public interface IBaseEntity<T> : IBaseEntity
{
    T Id { get; set; }
}
public interface IBaseEntity
{
}
