using Shared.Domain.Entities;
using Shared.Domain.Entities.Base;

namespace Appointments.Domain.Entities;

public class UserData : BaseEntity
{
	public UserData(string userId, string email, List<Role> roles)
	{
		UserId = userId;
		Email = email;
		Roles = roles;
	}
	private UserData() { }

	public string UserId { get; private set; }
	public string Email { get; private set; }
	public List<Role> Roles { get; private set; }
	
}
