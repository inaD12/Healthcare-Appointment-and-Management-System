using FluentValidation;
using Users.Application.Features.Users.Queries.GetById;
using Users.Domain.Utilities;

namespace Users.Application.Features.Users.Queries.GetUserById;

public class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
	public GetUserByIdQueryValidator()
	{
		RuleFor(q => q.Id)
		  .NotEmpty()
		  .MinimumLength(UsersBusinessConfiguration.ID_MIN_LENGTH)
		  .MaximumLength(UsersBusinessConfiguration.ID_MAX_LENGTH);
	}
}
