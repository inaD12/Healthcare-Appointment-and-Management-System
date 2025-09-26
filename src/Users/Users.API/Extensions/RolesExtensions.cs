using Shared.Domain.Entities;
using Users.Users.Models.Requests;

namespace Users.Extensions;

public static class RolesExtensions
{
	public static Role? MapRole(this Roles? role)
	{
		return role switch
		{
			Roles.Patient => Role.Patient,
			Roles.Doctor => Role.Doctor,
			Roles.Admin => Role.Administrator,
			null => null,
			_ => throw new ArgumentOutOfRangeException(nameof(role), role, "Invalid role")
		};
	}
	
	public static Role MapRole(this Roles role)
	{
		return role switch
		{
			Roles.Patient => Role.Patient,
			Roles.Doctor => Role.Doctor,
			Roles.Admin => Role.Administrator,
			_ => throw new ArgumentOutOfRangeException(nameof(role), role, "Invalid role")
		};
	}
}
