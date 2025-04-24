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
	private UserData() { }

	public string UserId { get; private set; }
	public string Email { get; private set; }
	public Roles Role { get; private set; }
}
