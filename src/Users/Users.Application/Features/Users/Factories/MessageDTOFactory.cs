using Users.Application.Features.Users.Factories.Abstractions;
using Users.Domain.DTOs.Responses;

namespace Users.Application.Features.Users.Factories;

public class MessageDTOFactory : IMessageDTOFactory
{
	public MessageDTO CreateMessage(string message)
	{
		return new MessageDTO(message);
	}
}
