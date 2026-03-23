
namespace Shared.Domain.Entities.Base;

public abstract class BaseConcurrencyEntity : BaseEntity
{
    public uint RowVersion { get; private set; }
}