namespace Patients.Domain.ValueObjects;

public sealed class Diagnosis
{
    public string IcdCode { get; }
    public string Description { get; }

    public Diagnosis(string icdCode, string description)
    {
        IcdCode = icdCode;
        Description = description;
    }
}
