using FluentValidation;
using Users.Domain.DTOs.Requests;

namespace Users.Application.Validators
{
	public class UpdateUserReqDTOValidator : AbstractValidator<UpdateUserReqDTO>
	{
		public UpdateUserReqDTOValidator()
		{
			RuleFor(x => x.NewEmail)
				.EmailAddress().WithMessage("Invalid email format.")
				.When(x => !string.IsNullOrEmpty(x.NewEmail));
		}
	}
}
