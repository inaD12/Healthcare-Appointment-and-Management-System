using Shared.Domain.Entities.Base;
using Shared.Domain.Enums;
using Shared.Domain.Results;
using Users.Domain.Events;
using Users.Domain.Responses;

namespace Users.Domain.Entities;

public sealed class User : BaseEntity
{
	public string Email { get; private set; }
	public Roles Role { get; private set; }
	public string FirstName { get; private set; }
	public string LastName { get; private set; }
	public DateTime DateOfBirth { get; private set; }
	public string? PhoneNumber { get; private set; }
	public string? Address { get; private set; }
	public bool EmailVerified { get; private set; }
	public string IdentityId { get; private set; }

	private User()
	{
	}

	private User(
		string email,
		Roles role,
		string firstName,
		string lastName,
		DateTime dateOfBirth,
		bool emailVerified,
		string identityId,
		string? phoneNumber,
		string? address)
	{
		Email = email;
		Role = role;
		FirstName = firstName;
		LastName = lastName;
		DateOfBirth = dateOfBirth;
		PhoneNumber = phoneNumber;
		Address = address;
		EmailVerified = emailVerified;
		IdentityId = identityId;
	}

	public static User Create(
		string email,
		Roles role,
		string firstName,
		string lastName,
		DateTime dateOfBirth,
		string identityId,
		string? phoneNumber,
		string? address)
	{
		var user = new User(
			email,
			role,
			firstName,
			lastName,
			dateOfBirth,
			false,
			identityId,
			phoneNumber,
			address);

		user.RaiseDomainEvent(new UserCreatedDomainEvent(user.Id, user.Email, user.Role));

		return user;
	}

	public Result VerifyEmail()
	{
		if (EmailVerified)
			return Result.Failure(ResponseList.EmailAlreadyVerified);

		EmailVerified = true;

		return Result.Success();
	}

	public void UpdateProfile(string? newEmail, string? firstName, string? lastName)
	{
		FirstName = firstName ?? FirstName;
		LastName = lastName ?? LastName;
		Email = newEmail ?? Email;
	}
}

