namespace Users.Application.Auth.TokenManager
{
	public interface ITokenManager
	{
		string CreateToken(string id);
	}
}