namespace Users.Application.Auth.Helpers;

public interface IJwtParser
{
	string GetIdFromToken();
}