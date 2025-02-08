using Shared.Domain.Enums;
using Users.Domain.Entities;

namespace Users.Application.Factories.Interfaces
{
    public interface IUserFactory
    {
        User CreateUser(string Email, string PasswordHash, string Salt, string FirstName, string LastName, DateTime DateOfBirth, string? PhoneNumber, string? Address, Roles Role, string? Id = null, bool EmailVerified = false);
    }
}