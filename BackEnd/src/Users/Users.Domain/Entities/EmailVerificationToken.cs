using Shared.Domain.Entities.Base;

namespace Users.Domain.Entities;

public class EmailVerificationToken : BaseEntity
{
	public string UserId { get; private set; }
	public DateTime CreatedOnUtc { get; private set; }
	public DateTime ExpiresOnUtc { get; private set; }
	public User? User { get; private set; }

	private EmailVerificationToken() { }
	private EmailVerificationToken(string userId, DateTime createdOnUtc, DateTime expiresOnUtc, User? user)
	{
		UserId = userId;
		CreatedOnUtc = createdOnUtc;
		ExpiresOnUtc = expiresOnUtc;
		User = user;
	}

	public static EmailVerificationToken Create(string userId, DateTime createdOnUtc, DateTime? expiresOnUtc = null, User? user = null)
	{
		return new EmailVerificationToken(
			userId,
			createdOnUtc,
			expiresOnUtc ?? createdOnUtc.AddDays(1),
			user);
	}
}
