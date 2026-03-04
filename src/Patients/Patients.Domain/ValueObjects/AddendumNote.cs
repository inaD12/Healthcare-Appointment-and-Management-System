namespace Patients.Domain.ValueObjects;

public sealed class AddendumNote
{
    public string Id { get; }
    public string Text { get; }
    public DateTime CreatedAt { get; }

    public AddendumNote(string text, DateTime createdAt)
    {
        Id = Guid.NewGuid().ToString();
        Text = text;
        CreatedAt = createdAt;
    }
}
