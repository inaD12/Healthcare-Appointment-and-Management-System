namespace Users.Users.Models.Requests;

public class UpdateCurrentUserRequest
{
	public string? NewEmail { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
}
