namespace Patients.Domain.ValueObjects;

public sealed class Prescription
{
    public string Id { get; }
    public string MedicationName { get; }
    public string Dosage { get; }
    public string Instructions { get; }

    public Prescription(string medicationName, string dosage, string instructions)
    {
        Id = Guid.NewGuid().ToString();
        MedicationName = medicationName;
        Dosage = dosage;
        Instructions = instructions;
    }
}
