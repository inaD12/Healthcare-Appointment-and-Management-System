namespace Patients.Domain.ValueObjects;

public sealed class Allergy
{
    public string Id { get; }
    public string Substance { get; }
    public string Reaction { get; }

    public Allergy(string substance, string reaction)
    {
        Id = Guid.NewGuid().ToString();
        Substance = substance;
        Reaction = reaction;
    }
}