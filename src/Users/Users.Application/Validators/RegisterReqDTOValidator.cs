using FluentValidation;
using Users.Domain.DTOs.Requests;
using Users.Domain.Utilities;

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
				.MinimumLength(UsersBusinessConfiguration.PASSWORD_MIN_LENGTH).WithMessage("Password must be at least 6 characters long.")
				.MaximumLength(UsersBusinessConfiguration.PASSWORD_MAX_LENGTH).WithMessage("Password musn't be more than 30 charecters long.");

			RuleFor(x => x.FirstName)
				.NotEmpty().WithMessage("First Name is required.")
				.MinimumLength(UsersBusinessConfiguration.FIRSTNAME_MIN_LENGTH).WithMessage("First name must be at least 3 characters long.")
				.MaximumLength(UsersBusinessConfiguration.FIRSTNAME_MAX_LENGTH).WithMessage("First name musn't be more than 30 charecters long.");

			RuleFor(x => x.LastName)
				.NotEmpty().WithMessage("Last Name is required.")
				.MinimumLength(UsersBusinessConfiguration.LASTTNAME_MIN_LENGTH).WithMessage("Last name must be at least 3 characters long.")
				.MaximumLength(UsersBusinessConfiguration.LASTNAME_MAX_LENGTH).WithMessage("Last name musn't be more than 30 charecters long.");

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
