﻿using Shared.Domain.Entities.Base;
using Shared.Domain.Enums;

namespace Users.Domain.Entities;

public class User : BaseEntity
{
	public string Email { get; set; }
	public string PasswordHash { get; set; }
	public string Salt { get; set; }
	public Roles Role { get; set; }
	public string FirstName { get; set; }
	public string LastName { get; set; }
	public DateTime DateOfBirth { get; set; }
	public string? PhoneNumber { get; set; }
	public string? Address { get; set; }
	public bool EmailVerified { get; set; }

	public User(
		string email,
		string passwordHash,
		string salt,
		Roles role,
		string firstName,
		string lastName,
		DateTime dateOfBirth,
		string? phoneNumber,
		string? address,
		bool emailVerified)
	{
		Email = email;
		PasswordHash = passwordHash;
		Salt = salt;
		Role = role;
		FirstName = firstName;
		LastName = lastName;
		DateOfBirth = dateOfBirth;
		PhoneNumber = phoneNumber;
		Address = address;
		EmailVerified = emailVerified;
	}
}
