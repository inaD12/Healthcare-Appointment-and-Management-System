using Users.Domain.DTOs.Responses;

namespace Users.Application.Factories.Interfaces;

public interface IMessageDTOFactory
{
	MessageDTO CreateMessage(string message);
}