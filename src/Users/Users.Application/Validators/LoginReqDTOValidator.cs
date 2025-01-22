using FluentValidation;
using Users.Domain.DTOs.Requests;

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
				.MinimumLength(6).WithMessage("Password must be at least 6 characters long.");
		}
	}
}
