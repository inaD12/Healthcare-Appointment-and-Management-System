using HotChocolate;

namespace Shared.Domain.Entities.Base;

public abstract class BaseConcurrencyEntity : BaseEntity
{
    [GraphQLIgnore]
    public uint RowVersion { get; private set; }
}