using Users.Domain.Entities;

namespace Users.Application.Factories.Interfaces;

public interface IEmailVerificationTokenFactory
{
	EmailVerificationToken CreateToken(string Id, string userId, DateTime CreatedOnUtc, DateTime ExpiresOnUtc, User? User = null);
}