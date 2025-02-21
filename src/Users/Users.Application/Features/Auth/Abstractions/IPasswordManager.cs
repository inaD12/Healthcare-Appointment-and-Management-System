namespace Users.Application.Features.Auth.Abstractions;

public interface IPasswordManager
{
	string HashPassword(string password, out string salt);
	bool VerifyPassword(string password, string hash, string salt);
}