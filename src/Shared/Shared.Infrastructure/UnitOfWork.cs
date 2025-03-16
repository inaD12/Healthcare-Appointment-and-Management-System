using MediatR;
using Microsoft.EntityFrameworkCore;
using Shared.Domain.Abstractions;
using Shared.Domain.Entities.Base;
using Shared.Domain.Exceptions;

namespace Shared.Infrastructure;

internal class UnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
{
	private readonly TContext _dbContext;
	private readonly IMediator _notificationPublisher;

	public UnitOfWork(TContext dbContext, IMediator notificationPublisher)
	{
		_dbContext = dbContext;
		_notificationPublisher = notificationPublisher;
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
			throw new ConcurrencyException("Concurrency exception occurred.", ex);
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
			await _notificationPublisher.Publish(domainEvent, cancellationToken);
		}
	}
}