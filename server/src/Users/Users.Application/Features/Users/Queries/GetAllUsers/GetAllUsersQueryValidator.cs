using FluentValidation;
using Users.Domain.Entities;
using Users.Domain.Utilities;

namespace Users.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
{
	public GetAllUsersQueryValidator()
	{
		RuleFor(q => q.Email)
		   .MinimumLength(UsersBusinessConfiguration.EMAIL_MIN_LENGTH)
		   .MaximumLength(UsersBusinessConfiguration.EMAIL_MAX_LENGTH)
		   .When(q => !string.IsNullOrEmpty(q.Email));

		RuleFor(q => q.Role)
		   .IsInEnum()
			.When(q => q.Role.HasValue);

		RuleFor(q => q.FirstName)
		   .MinimumLength(UsersBusinessConfiguration.FIRSTNAME_MIN_LENGTH)
		   .MaximumLength(UsersBusinessConfiguration.FIRSTNAME_MAX_LENGTH)
		   .When(q => !string.IsNullOrEmpty(q.FirstName));

		RuleFor(q => q.LastName)
		   .MinimumLength(UsersBusinessConfiguration.LASTTNAME_MIN_LENGTH)
		   .MaximumLength(UsersBusinessConfiguration.LASTNAME_MAX_LENGTH)
		   .When(q => !string.IsNullOrEmpty(q.LastName));

		RuleFor(q => q.PhoneNumber)
		   .MinimumLength(UsersBusinessConfiguration.PHONENUMBER_MIN_LENGTH)
		   .MaximumLength(UsersBusinessConfiguration.PHONENUMBER_MAX_LENGTH)
		   .When(q => !string.IsNullOrEmpty(q.PhoneNumber));

		RuleFor(q => q.Address)
		   .MinimumLength(UsersBusinessConfiguration.ADRESS_MIN_LENGTH)
		   .MaximumLength(UsersBusinessConfiguration.ADRESS_MAX_LENGTH)
		   .When(q => !string.IsNullOrEmpty(q.Address));

		RuleFor(q => q.SortPropertyName)
			.Must(BeAValidSortProperty).WithMessage("SortPropertyName must be a valid property");
	}

	private bool BeAValidSortProperty(string propertyName)
	{
		var isValidProperty = typeof(User).GetProperties().Any(e => e.Name == propertyName);

		return isValidProperty;
	}
}
