using Shared.Domain.Entities;
using Shared.Domain.Enums;

namespace Shared.Domain.Extensions;

public static class RolesExtensions
{
	public static Role? MapToRole(this Roles? role)
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
	
	public static Role MapToRole(this Roles role)
	{
		return role switch
		{
			Roles.Patient => Role.Patient,
			Roles.Doctor => Role.Doctor,
			Roles.Admin => Role.Administrator,
			_ => throw new ArgumentOutOfRangeException(nameof(role), role, "Invalid role")
		};
	}
	
	public static Roles MapToRoleEnum(this Role role)
	{
		return role switch
		{
			var r when r.Name == Role.Patient.Name => Roles.Patient,
			var r when r.Name == Role.Doctor.Name => Roles.Doctor,
			var r when r.Name == Role.Administrator.Name => Roles.Admin,
			_ => throw new ArgumentOutOfRangeException(nameof(role), role, "Invalid role")
		};
	}
}
