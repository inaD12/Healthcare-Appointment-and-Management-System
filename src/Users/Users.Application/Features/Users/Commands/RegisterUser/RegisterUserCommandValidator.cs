using System.Reflection;
using FluentValidation;
using Shared.Domain.Entities;
using Users.Domain.Utilities;

namespace Users.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
	private static HashSet<string> _validRoles;
	public RegisterUserCommandValidator()
	{
		RuleFor(x => x.Email)
			.NotEmpty()
			.MinimumLength(UsersBusinessConfiguration.EMAIL_MIN_LENGTH)
			.MaximumLength(UsersBusinessConfiguration.EMAIL_MAX_LENGTH)
			.EmailAddress();

		RuleFor(x => x.Password)
			.NotEmpty()
			.MinimumLength(UsersBusinessConfiguration.PASSWORD_MIN_LENGTH)
			.MaximumLength(UsersBusinessConfiguration.PASSWORD_MAX_LENGTH);

		RuleFor(x => x.FirstName)
			.NotEmpty()
			.MinimumLength(UsersBusinessConfiguration.FIRSTNAME_MIN_LENGTH)
			.MaximumLength(UsersBusinessConfiguration.FIRSTNAME_MAX_LENGTH);

		RuleFor(x => x.LastName)
			.NotEmpty()
			.MinimumLength(UsersBusinessConfiguration.LASTTNAME_MIN_LENGTH)
			.MaximumLength(UsersBusinessConfiguration.LASTNAME_MAX_LENGTH);

		RuleFor(x => x.DateOfBirth)
			.NotEmpty()
			.LessThan(DateTime.Now);

		RuleFor(x => x.PhoneNumber)
			.NotEmpty()
			.Matches(@"^\+?[0-9\s\-().]*$")
			.MinimumLength(UsersBusinessConfiguration.PHONENUMBER_MIN_LENGTH)
			.MaximumLength(UsersBusinessConfiguration.PHONENUMBER_MAX_LENGTH);

		RuleFor(x => x.Address)
			.NotEmpty()
			.MinimumLength(UsersBusinessConfiguration.ADRESS_MIN_LENGTH)
			.MaximumLength(UsersBusinessConfiguration.ADRESS_MAX_LENGTH);

		_validRoles = typeof(Role)
			.GetFields(BindingFlags.Public | BindingFlags.Static)
			.Where(f => f.FieldType == typeof(Role))
			.Select(f => ((Role)f.GetValue(null)!).Name)
			.ToHashSet();

		RuleFor(x => x.Role)
			.Must(role => _validRoles.Contains(role.Name));
	}
}
