namespace Patients.Domain.ValueObjects;

public sealed class ChronicCondition
{
    public string Id { get; }
    public string Name { get; }

    public ChronicCondition(string name)
    {
        Id = Guid.NewGuid().ToString();
        Name = name;
    }
}