namespace Users.Users.Models.Requests;

public sealed record UpdateCurrentUserRequest(
	string? NewEmail,
	string? FirstName,
	string? LastName
);
