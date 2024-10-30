using Users.Application.Factories.Interfaces;
using Users.Domain.DTOs.Responses;

namespace Users.Application.Factories
{
    public class MessageDTOFactory : IMessageDTOFactory
	{
		public MessageDTO CreateMessage(string message)
		{
			return new MessageDTO(message);
		}
	}
}
