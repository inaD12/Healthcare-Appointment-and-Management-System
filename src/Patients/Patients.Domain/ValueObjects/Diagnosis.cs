namespace Patients.Domain.ValueObjects;

public sealed class Diagnosis
{
    public string Id { get; }
    public string IcdCode { get; }
    public string Description { get; }

    public Diagnosis(string icdCode, string description)
    {
        Id = Guid.NewGuid().ToString();
        IcdCode = icdCode;
        Description = description;
    }
}
