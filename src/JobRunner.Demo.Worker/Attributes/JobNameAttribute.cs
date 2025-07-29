namespace JobRunner.Demo.Worker.Attributes;

[AttributeUsage(AttributeTargets.Class)]
internal sealed class JobNameAttribute : Attribute
{
    public string Name { get; }
    public JobNameAttribute(string name)
    {
        Name = name;
    }
}
