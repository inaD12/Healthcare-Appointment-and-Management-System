using System.ComponentModel.DataAnnotations;

namespace Shared.Domain.Entities.Base;

public abstract class BaseConcurrencyEntity : BaseEntity
{
    [Timestamp]
    public byte[] RowVersion { get; private set; } = default!;
}