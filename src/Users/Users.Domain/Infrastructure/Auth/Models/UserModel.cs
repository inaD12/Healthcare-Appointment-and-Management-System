namespace Users.Domain.Infrastructure.Auth.Models;

public sealed record UserModel(string Email, string Password, string FirstName, string LastName);
