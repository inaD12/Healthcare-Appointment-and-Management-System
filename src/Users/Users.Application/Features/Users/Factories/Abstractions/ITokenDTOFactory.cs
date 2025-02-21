using Users.Domain.DTOs.Responses;

namespace Users.Application.Features.Users.Factories.Abstractions;

public interface ITokenDTOFactory
{
	TokenDTO CreateToken(string token);
}