namespace Patients.Domain.ValueObjects;

public sealed class AddendumNote
{
    public string Text { get; }
    public DateTime CreatedAt { get; }

    public AddendumNote(string text, DateTime createdAt)
    {
        Text = text;
        CreatedAt = createdAt;
    }
}
