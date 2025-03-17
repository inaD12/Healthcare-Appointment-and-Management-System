namespace Users.Domain.Auth.Models;

public record PasswordHashResult(string PasswordHash, string Salt);
