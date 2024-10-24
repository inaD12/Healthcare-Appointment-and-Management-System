using FluentValidation;
using Users.Domain.DTOs.Requests;

namespace Users.Application.Validators
{
	public class RegisterReqDTOValidator : AbstractValidator<RegisterReqDTO>
	{
		public RegisterReqDTOValidator()
		{
			RuleFor(x => x.Email)
				.NotEmpty().WithMessage("Email is required.")
				.EmailAddress().WithMessage("Invalid email format.");

			RuleFor(x => x.Password)
				.NotEmpty().WithMessage("Password is required.")
				.MinimumLength(6).WithMessage("Password must be at least 6 characters long.");

			RuleFor(x => x.FirstName)
				.NotEmpty().WithMessage("First Name is required.");

			RuleFor(x => x.LastName)
				.NotEmpty().WithMessage("Last Name is required.");

			RuleFor(x => x.DateOfBirth)
				.NotEmpty().WithMessage("Date of Birth is required.")
				.LessThan(DateTime.Now).WithMessage("Date of Birth must be in the past.");

			RuleFor(x => x.PhoneNumber)
				.NotEmpty().WithMessage("Phone Number is required.")
				.Matches(@"^\+?[0-9]\d{1,14}$").WithMessage("Invalid phone number format.");

			RuleFor(x => x.Address)
				.NotEmpty().WithMessage("Address is required.");
		}
	}
}
