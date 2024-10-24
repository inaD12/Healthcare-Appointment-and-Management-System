using Users.Domain.Entities;

namespace Users.Domain.EmailVerification
{
	public class EmailVerificationToken
	{
		public string Id { get; set; }
		public string UserId { get; set; }
		public DateTime CreatedOnUtc { get; set; }
		public DateTime ExpiresOnUtc { get; set; }
		public User User { get; set; }
	}
}
