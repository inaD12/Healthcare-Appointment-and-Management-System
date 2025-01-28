using FluentValidation;
using Users.Domain.DTOs.Requests;
using Users.Domain.Utilities;

namespace Users.Application.Validators
{
	public class LoginReqDTOValidator : AbstractValidator<LoginReqDTO>
	{
		public LoginReqDTOValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email is required.")
				.EmailAddress().WithMessage("A valid email is required.");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Password is required.")
				.MinimumLength(UsersBusinessConfiguration.PASSWORD_MIN_LENGTH).WithMessage("Password must be at least 6 characters long.")
				.MaximumLength(UsersBusinessConfiguration.PASSWORD_MAX_LENGTH).WithMessage("Password musn't be more than 30 charecters long.");
		}
	}
}
