using FluentValidation;
using Users.Domain.Utilities;

namespace Users.Application.Features.Users.Commands.DeleteUser;

public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
	public DeleteUserCommandValidator()
	{
		RuleFor(x => x.Id)
				.NotEmpty()
				.MinimumLength(UsersBusinessConfiguration.ID_MIN_LENGTH)
				.MaximumLength(UsersBusinessConfiguration.ID_MAX_LENGTH);
	}
}
