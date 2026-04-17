using System.Reflection;
using FluentValidation;
using Shared.Domain.Entities;
using Users.Domain.Entities;
using Users.Domain.Utilities;

namespace Users.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryValidator : AbstractValidator<GetAllUsersQuery>
{
	public GetAllUsersQueryValidator()
	{
		var roles = typeof(Role)
			.GetFields(BindingFlags.Public | BindingFlags.Static)
			.Where(f => f.FieldType == typeof(Role))
			.Select(f => ((Role)f.GetValue(null)!).Name)
			.ToHashSet();
		
		RuleFor(q => q.Role)
			.Must(role => roles.Contains(role!.Name))
			.When(q => q.Role != null);

		RuleFor(q => q.SortPropertyName)
			.Must(BeAValidSortProperty).WithMessage("SortPropertyName must be a valid property");
	}

	private bool BeAValidSortProperty(string propertyName)
	{
		var isValidProperty = typeof(User).GetProperties().Any(e => e.Name == propertyName);

		return isValidProperty;
	}
}
