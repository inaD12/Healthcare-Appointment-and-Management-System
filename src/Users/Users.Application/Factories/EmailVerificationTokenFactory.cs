using Users.Application.Factories.Interfaces;
using Users.Domain.Entities;

namespace Users.Application.Factories;

public class EmailVerificationTokenFactory : IEmailVerificationTokenFactory
{
	public EmailVerificationToken CreateToken(string Id, string userId, DateTime CreatedOnUtc, DateTime ExpiresOnUtc, User? User = null)
	{
		return new EmailVerificationToken(Id, userId, CreatedOnUtc, ExpiresOnUtc, User);
	}
}
