using Shared.Domain.Entities.Base;
using Users.Domain.Entities;

namespace Users.Domain.EmailVerification
{
	public class EmailVerificationToken : BaseEntity
	{
		public string UserId { get; set; }
		public DateTime CreatedOnUtc { get; set; }
		public DateTime ExpiresOnUtc { get; set; }
		public User User { get; set; }

		public EmailVerificationToken() { }
		public EmailVerificationToken(string id, string userId, DateTime createdOnUtc, DateTime expiresOnUtc, User user)
		{
			Id = id;
			UserId = userId;
			CreatedOnUtc = createdOnUtc;
			ExpiresOnUtc = expiresOnUtc;
			User = user;
		}
	}

}
