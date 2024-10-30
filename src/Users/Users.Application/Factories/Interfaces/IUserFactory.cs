﻿using Users.Domain.Entities;

namespace Users.Application.Factories.Interfaces
{
    public interface IUserFactory
    {
        User CreateUser(string Email, string PasswordHash, string Salt, string FirstName, string LastName, DateTime DateOfBirth, string PhoneNumber, string Address, string? Id = null, string Role = "User", bool EmailVerified = false);
    }
}