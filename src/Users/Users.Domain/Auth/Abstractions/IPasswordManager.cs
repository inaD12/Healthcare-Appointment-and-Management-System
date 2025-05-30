using Users.Domain.Auth.Models;

namespace Users.Domain.Auth.Abstractions;

public interface IPasswordManager
{
	PasswordHashResult HashPassword(string password);
	bool VerifyPassword(string password, string hash, string salt);
}