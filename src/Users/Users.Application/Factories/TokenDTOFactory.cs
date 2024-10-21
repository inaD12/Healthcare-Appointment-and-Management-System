using Users.Domain.DTOs.Responses;

namespace Users.Application.Factories
{
	public class TokenDTOFactory : ITokenDTOFactory
	{
		public TokenDTO CreateToken(string token)
		{
			return new TokenDTO(token);
		}
	}
}
