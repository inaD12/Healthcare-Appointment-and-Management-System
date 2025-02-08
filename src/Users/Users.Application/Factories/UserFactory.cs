using Shared.Domain.Enums;
using Users.Application.Factories.Interfaces;
using Users.Domain.Entities;

namespace Users.Application.Factories
{
    public class UserFactory : IUserFactory
	{
		public User CreateUser(
			string Email,
			string PasswordHash,
			string Salt,
			string FirstName,
			string LastName,
			DateTime DateOfBirth,
			string? PhoneNumber,
			string? Address,
			Roles Role,
			string? Id = null,
			bool EmailVerified = false)
		{
			return new User(Id ?? Guid.NewGuid().ToString(), Email, PasswordHash, Salt, Role, FirstName, LastName, DateOfBirth.ToUniversalTime(), PhoneNumber, Address, EmailVerified);
		}
	}
}
