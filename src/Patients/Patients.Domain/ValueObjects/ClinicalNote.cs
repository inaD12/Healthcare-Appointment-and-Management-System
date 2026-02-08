namespace Patients.Domain.ValueObjects;

public sealed class ClinicalNote
{
    public string Text { get; }
    public DateTime CreatedAt { get; }

    public ClinicalNote(string text, DateTime createdAt)
    {
        Text = text;
        CreatedAt = createdAt;
    }
}
