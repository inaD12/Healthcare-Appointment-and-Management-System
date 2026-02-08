namespace Patients.Domain.ValueObjects;

public sealed class ChronicCondition
{
    public string Name { get; }

    public ChronicCondition(string name)
    {
        Name = name;
    }
}