using FluentValidation;
using Users.Domain.Utilities;

namespace Users.Application.Features.Users.Commands.RegisterUser;

public class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
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
			.Matches(@"^\+?\d{5,18}$")
			.MinimumLength(UsersBusinessConfiguration.PHONENUMBER_MIN_LENGTH)
			.MaximumLength(UsersBusinessConfiguration.PHONENUMBER_MAX_LENGTH);

		RuleFor(x => x.Address)
			.MinimumLength(UsersBusinessConfiguration.ADRESS_MIN_LENGTH)
			.MaximumLength(UsersBusinessConfiguration.ADRESS_MAX_LENGTH);
	}
}
