namespace Users.Application.Auth.PasswordManager;

public interface IPasswordManager
{
	string HashPassword(string password, out string salt);
	bool VerifyPassword(string password, string hash, string salt);
}