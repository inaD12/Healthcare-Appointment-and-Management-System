using MassTransit;
using MediatR;
using Shared.Domain.Events;
using Users.Application.Commands.Email.SendEmail;

namespace Users.Application.Consumers;

public sealed class UserCreatedConsumer : IConsumer<UserCreatedEvent>
{
	private readonly ISender _sender;

	public UserCreatedConsumer(ISender sender)
	{
		_sender = sender;
	}

	public async Task Consume(ConsumeContext<UserCreatedEvent> context)
	{
		var message = context.Message;

		var command = new SendEmailCommand(
			message.UserId,
			message.Email
			);

		await _sender.Send(command);
	}
}
