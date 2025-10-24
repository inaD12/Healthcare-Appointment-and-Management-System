using Shared.Domain.Entities;
using Shared.Domain.Entities.Base;
using Shared.Domain.Enums;

namespace Appointments.Domain.Entities;

public class UserData : BaseEntity
{
	public UserData(string userId, string email, List<Roles> roles)
	{
		UserId = userId;
		Email = email;
		Roles = roles;
	}
	private UserData() { }

	public string UserId { get; private set; }
	public string Email { get; private set; }
	public List<Roles> Roles { get; private set; }
	
}
