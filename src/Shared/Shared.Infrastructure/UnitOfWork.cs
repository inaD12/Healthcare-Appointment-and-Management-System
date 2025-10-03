using MassTransit;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Abstractions;
using Shared.Domain.Entities.Base;

namespace Shared.Infrastructure;

internal class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
	private readonly TContext _dbContext;
	private readonly IEventBus _eventBus;

	public UnitOfWork(TContext dbContext, IEventBus eventBus)
	{
		_dbContext = dbContext;
		_eventBus = eventBus;
	}

	public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
	{
		try
		{
			await PublishDomainEventsAsync(cancellationToken);

			await _dbContext.SaveChangesAsync(cancellationToken);
		}
		catch (DbUpdateConcurrencyException ex)
		{
			throw new Domain.Exceptions.ConcurrencyException("Concurrency exception occurred.", ex);
		}
	}

	private async Task PublishDomainEventsAsync(CancellationToken cancellationToken)
	{
		var domainEvents = _dbContext.ChangeTracker
			.Entries<BaseEntity>()
			.Select(e => e.Entity)
			.SelectMany(e =>
			{
				var domainEvents = e.GetDomainEvents();

				e.ClearDomainEvents();

				return domainEvents;
			})
			.ToList();

		foreach (var domainEvent in domainEvents)
		{
			Type concreteType = domainEvent.GetType();

			var publishMethod = typeof(IEventBus)
				.GetMethod(nameof(IEventBus.PublishAsync))!
				.MakeGenericMethod(concreteType);

			await (Task)publishMethod.Invoke(_eventBus, [domainEvent, cancellationToken])!;
		}
	}
}