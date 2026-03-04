using Shared.Domain.Abstractions;

namespace Shared.Domain.Entities.Base;

public abstract class BaseEntity
{
	private readonly List<IDomainEvent> _domainEvents = new();
	public BaseEntity()
	{
		Id = Guid.NewGuid().ToString();
	}
	public string Id { get; set; }

	public IReadOnlyList<IDomainEvent> GetDomainEvents()
	{
		return _domainEvents.ToList();
	}

	public void ClearDomainEvents()
	{
		_domainEvents.Clear();
	}

	protected void RaiseDomainEvent(IDomainEvent domainEvent)
	{
		_domainEvents.Add(domainEvent);
	}
}
