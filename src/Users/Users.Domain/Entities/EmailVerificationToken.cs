﻿using Shared.Domain.Entities.Base;

namespace Users.Domain.Entities;

public class EmailVerificationToken : BaseEntity
{
	public string UserId { get; set; }
	public DateTime CreatedOnUtc { get; set; }
	public DateTime ExpiresOnUtc { get; set; }
	public User User { get; set; }

	public EmailVerificationToken() { }
	public EmailVerificationToken(string userId, DateTime createdOnUtc, DateTime expiresOnUtc, User user)
	{
		UserId = userId;
		CreatedOnUtc = createdOnUtc;
		ExpiresOnUtc = expiresOnUtc;
		User = user;
	}
}
