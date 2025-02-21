using Users.Domain.DTOs.Responses;

namespace Users.Application.Features.Users.Factories.Abstractions;

public interface IMessageDTOFactory
{
	MessageDTO CreateMessage(string message);
}