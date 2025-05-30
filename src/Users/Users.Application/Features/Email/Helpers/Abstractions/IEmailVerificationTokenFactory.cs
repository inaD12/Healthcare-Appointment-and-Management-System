using Users.Domain.Entities;

namespace Users.Application.Features.Email.Helpers.Abstractions;

public interface IEmailVerificationTokenFactory
{
	EmailVerificationToken CreateToken(string userId, DateTime CreatedOnUtc, DateTime? ExpiresOnUtc = null, User? User = null);
}