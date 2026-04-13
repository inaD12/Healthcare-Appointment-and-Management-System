namespace Users.Users.Models.Requests;

public sealed record UpdateCurrentUserRequest(
	string? FirstName,
	string? LastName
);
