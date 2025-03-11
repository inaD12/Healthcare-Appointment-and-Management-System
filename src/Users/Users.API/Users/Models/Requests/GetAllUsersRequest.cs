using Shared.API.Models.Requests;
using Shared.Domain.Enums;

namespace Users.Users.Models.Requests;

public class GetAllUsersRequest : CollectionReadRequest
{
	public string? Email { get; set; } = string.Empty;
	public Roles? Role { get; set; }
	public string? FirstName { get; set; } = string.Empty;
	public string? LastName { get; set; } = string.Empty;
	public string? PhoneNumber { get; set; } = string.Empty;
	public string? Address { get; set; } = string.Empty;
	public bool? EmailVerified { get; set; }
}
