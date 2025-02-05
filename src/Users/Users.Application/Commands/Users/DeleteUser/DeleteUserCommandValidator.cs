using FluentValidation;
using Users.Application.Commands.Users.DeleteUser;
using Users.Domain.Utilities;

namespace Users.Application.Commands.Users.UpdateUser;

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
