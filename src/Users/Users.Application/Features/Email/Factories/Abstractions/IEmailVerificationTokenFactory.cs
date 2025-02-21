using Users.Domain.Entities;

namespace Users.Application.Features.Email.Factories.Abstractions;

public interface IEmailVerificationTokenFactory
{
	EmailVerificationToken CreateToken(string Id, string userId, DateTime CreatedOnUtc, DateTime ExpiresOnUtc, User? User = null);
}