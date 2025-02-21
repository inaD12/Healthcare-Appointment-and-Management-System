using Users.Application.Features.Email.Factories.Abstractions;
using Users.Domain.Entities;

namespace Users.Application.Features.Email.Factories;

public class EmailVerificationTokenFactory : IEmailVerificationTokenFactory
{
	public EmailVerificationToken CreateToken(string Id, string userId, DateTime CreatedOnUtc, DateTime ExpiresOnUtc, User? User = null)
	{
		return new EmailVerificationToken(Id, userId, CreatedOnUtc, ExpiresOnUtc, User);
	}
}
