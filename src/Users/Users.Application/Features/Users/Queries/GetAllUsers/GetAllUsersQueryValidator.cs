using FluentValidation;
using Users.Domain.Entities;

namespace Users.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
{
	public GetAllUsersQueryValidator()
	{
		RuleFor(q => q.SortPropertyName)
			.Must(BeAValidSortProperty).WithMessage("SortPropertyName must be a valid property");
	}

	private bool BeAValidSortProperty(string propertyName)
	{
		var isValidProperty = typeof(User).GetProperties().Any(e => e.Name == propertyName);

		return isValidProperty;
	}
}
