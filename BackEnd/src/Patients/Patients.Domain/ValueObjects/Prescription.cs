using Shared.Domain.Entities;

namespace Patients.Domain.ValueObjects;

public sealed class Prescription: SoftDeletableEntity
{
    public string Id { get; }
    public string MedicationName { get; }
    public string Dosage { get; }
    public string Instructions { get; }
    public DateTime CreatedAt { get; }

    public Prescription(string medicationName, string dosage, string instructions, DateTime createdAt)
    {
        Id = Guid.NewGuid().ToString();
        MedicationName = medicationName;
        Dosage = dosage;
        Instructions = instructions;
        CreatedAt = createdAt;
    }
}
