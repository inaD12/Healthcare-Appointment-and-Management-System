using Shared.Domain.Entities.Base;
using Shared.Domain.Enums;

namespace Appointments.Domain.Entities;

public class UserData : BaseEntity
{
	public UserData(string userId, string email, Roles role)
	{
		UserId = userId;
		Email = email;
		Role = role;
	}

	public string UserId { get; set; }
	public string Email { get; set; }
	public Roles Role { get; set; }
}
