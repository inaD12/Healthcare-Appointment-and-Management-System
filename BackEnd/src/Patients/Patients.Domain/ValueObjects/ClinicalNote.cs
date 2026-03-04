namespace Patients.Domain.ValueObjects;

public sealed class ClinicalNote
{
    public string Id { get; }
    public string Text { get; }
    public DateTime CreatedAt { get; }

    public ClinicalNote(string text, DateTime createdAt)
    {
        Id = Guid.NewGuid().ToString();
        Text = text;
        CreatedAt = createdAt;
    }
}
