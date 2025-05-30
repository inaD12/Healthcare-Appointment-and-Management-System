using Users.Application.Features.Email.Helpers.Abstractions;
using Users.Domain.Entities;

namespace Users.Application.Features.Email.Helpers;

public class EmailVerificationTokenFactory : IEmailVerificationTokenFactory
{
	public EmailVerificationToken CreateToken(string userId, DateTime CreatedOnUtc, DateTime? ExpiresOnUtc = null, User? User = null)
	{
		return new EmailVerificationToken(
			userId,
			CreatedOnUtc,
			ExpiresOnUtc ?? CreatedOnUtc.AddDays(1),
			User);
	}
}
