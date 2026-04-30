namespace Shared.Domain.Entities;

public abstract class SoftDeletableEntity
{
    public bool IsDeleted { get; private set; }
    public DateTime? DeletedAt { get; private set; }

    public void Delete(DateTime utcNow)
    {
        if (IsDeleted)
            return;

        IsDeleted = true;
        DeletedAt = utcNow;
    }

    public void Restore()
    {
        if (!IsDeleted)
            return;

        IsDeleted = false;
        DeletedAt = null;
    }
}