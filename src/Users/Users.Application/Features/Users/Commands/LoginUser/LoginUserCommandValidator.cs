using FluentValidation;
using Users.Application.Features.Auth.Models;
using Users.Domain.Utilities;

namespace Users.Application.Features.Users.LoginUser;

public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
	public LoginUserCommandValidator()
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
	}
}
