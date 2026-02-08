namespace Patients.Domain.ValueObjects;

public sealed class Allergy
{
    public string Substance { get; }
    public string Reaction { get; }

    public Allergy(string substance, string reaction)
    {
        Substance = substance;
        Reaction = reaction;
    }
}