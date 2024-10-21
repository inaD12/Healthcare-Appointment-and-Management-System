using Users.Domain.DTOs.Responses;

namespace Users.Application.Factories
{
	public interface IMessageDTOFactory
	{
		MessageDTO CreateMessage(string message);
	}
}