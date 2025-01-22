using Users.Domain.DTOs.Responses;

namespace Users.Application.Auth.TokenManager
{
	public interface ITokenManager
	{
		TokenDTO CreateToken(string id);
	}
}