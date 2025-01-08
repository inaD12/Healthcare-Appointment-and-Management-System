﻿using Contracts.Abstractions.Messaging;
using Contracts.Results;
using Users.Application.Managers.Interfaces;
using Users.Domain.Entities;
using Users.Domain.Responses;

namespace Users.Application.Commands.Users.UpdateUser
{
	public sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
	{
		private readonly IRepositoryManager _repositotyManager;

		public UpdateUserCommandHandler(IRepositoryManager repositotyManager)
		{
			_repositotyManager = repositotyManager;
		}

		public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
		{
			var userResult = await _repositotyManager.User.GetUserByIdAsync(request.Id);
			if (userResult.IsFailure)
				return Result.Failure(userResult.Response);

			User user = userResult.Value;

			if (!string.IsNullOrEmpty(request.NewEmail) && request.NewEmail != user.Email)
			{
				var emailCheckResult = await _repositotyManager.User.GetUserByEmailAsync(request.NewEmail);
				if (emailCheckResult.IsSuccess)
					return Result.Failure(Responses.EmailTaken);

				user.Email = request.NewEmail;
			}

			user.FirstName = request.FirstName ?? user.FirstName;
			user.LastName = request.LastName ?? user.LastName;

			await _repositotyManager.User.UpdateUserAsync(user);

			return Result.Success(Responses.UpdateSuccessful);
		}
	}
}