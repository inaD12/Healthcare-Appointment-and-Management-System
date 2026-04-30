namespace Users.Users.Models.Requests;

public sealed record UpdateUserRequest(
	string? FirstName,
	string? LastName
);
