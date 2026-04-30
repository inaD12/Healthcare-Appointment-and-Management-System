using System.Reflection;
using FluentValidation;
using Shared.Domain.Entities;
using Users.Application.Features.Users.Commands.RegisterUser;

namespace Users.Application.Features.Users.Commands.RegisterUserByAdmin;

public class RegisterUserByAdminCommandValidator : AbstractValidator<RegisterUserCommand>
{
	public RegisterUserByAdminCommandValidator()
	{
		RuleFor(x => x.DateOfBirth)
			.NotEmpty()
			.LessThan(DateTime.Now);

		var validRoles = typeof(Role)
			.GetFields(BindingFlags.Public | BindingFlags.Static)
			.Where(f => f.FieldType == typeof(Role))
			.Select(f => ((Role)f.GetValue(null)!).Name)
			.ToHashSet();

		RuleFor(x => x.Role)
			.Must(role => validRoles.Contains(role.Name));
	}
}
