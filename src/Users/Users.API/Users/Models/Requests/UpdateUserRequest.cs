namespace Users.Users.Models.Requests;

public sealed record UpdateUserRequest(
	string? NewEmail,
	string? FirstName,
	string? LastName
);
