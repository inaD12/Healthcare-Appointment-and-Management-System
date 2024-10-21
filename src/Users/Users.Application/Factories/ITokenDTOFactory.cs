using Users.Domain.DTOs.Responses;

namespace Users.Application.Factories
{
	public interface ITokenDTOFactory
	{
		TokenDTO CreateToken(string token);
	}
}