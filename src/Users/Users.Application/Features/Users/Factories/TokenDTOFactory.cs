using Users.Application.Features.Users.Factories.Abstractions;
using Users.Domain.DTOs.Responses;

namespace Users.Application.Features.Users.Factories;

public class TokenDTOFactory : ITokenDTOFactory
{
	public TokenDTO CreateToken(string token)
	{
		return new TokenDTO(token);
	}
}
