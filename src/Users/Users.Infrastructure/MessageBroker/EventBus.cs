using MassTransit;

namespace Users.Infrastructure.MessageBroker;

public sealed class EventBus : IEventBus
{
	private readonly IPublishEndpoint _endpoint;

	public EventBus(IPublishEndpoint endpoint)
	{
		_endpoint = endpoint;
	}

	public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default) where T : class
	{
		return _endpoint.Publish(message, cancellationToken);
	}
}
