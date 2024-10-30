using Users.Domain.Entities;

namespace Users.Domain.EmailVerification
{
	public record EmailVerificationToken(string Id, string UserId, DateTime CreatedOnUtc, DateTime ExpiresOnUtc, User User);
}
