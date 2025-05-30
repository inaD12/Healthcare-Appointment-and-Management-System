﻿using FluentValidation;
using Users.Domain.Utilities;

namespace Users.Application.Features.Users.UpdateUser;

public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
	public UpdateUserCommandValidator()
	{
		RuleFor(x => x.Id)
			.NotEmpty()
			.MinimumLength(UsersBusinessConfiguration.ID_MIN_LENGTH)
			.MaximumLength(UsersBusinessConfiguration.ID_MAX_LENGTH);

		RuleFor(x => x.NewEmail)
			.MinimumLength(UsersBusinessConfiguration.EMAIL_MIN_LENGTH)
			.MaximumLength(UsersBusinessConfiguration.EMAIL_MAX_LENGTH)
			.EmailAddress();

		RuleFor(x => x.FirstName)
			.MinimumLength(UsersBusinessConfiguration.FIRSTNAME_MIN_LENGTH)
			.MaximumLength(UsersBusinessConfiguration.FIRSTNAME_MAX_LENGTH);

		RuleFor(x => x.LastName)
			.MinimumLength(UsersBusinessConfiguration.LASTTNAME_MIN_LENGTH)
			.MaximumLength(UsersBusinessConfiguration.LASTNAME_MAX_LENGTH);

		RuleFor(x => x)
			.Must(x => x.NewEmail != null || x.FirstName != null || x.LastName != null)
			.WithMessage("At least one field must be changed.");
	}
}
