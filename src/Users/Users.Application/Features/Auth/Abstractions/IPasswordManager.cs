using Users.Application.Features.Auth.Models;

namespace Users.Application.Features.Auth.Abstractions;

public interface IPasswordManager
{
	PasswordHashResult HashPassword(string password);
	bool VerifyPassword(string password, string hash, string salt);
}