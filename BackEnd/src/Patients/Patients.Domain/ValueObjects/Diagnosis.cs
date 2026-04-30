using Shared.Domain.Entities;

namespace Patients.Domain.ValueObjects;

public sealed class Diagnosis: SoftDeletableEntity
{
    public string Id { get; }
    public string IcdCode { get; }
    public string Description { get; }
    public DateTime CreatedAt { get; }

    public Diagnosis(string icdCode, string description, DateTime createdAt)
    {
        Id = Guid.NewGuid().ToString();
        IcdCode = icdCode;
        Description = description;
        CreatedAt = createdAt;
    }
}
