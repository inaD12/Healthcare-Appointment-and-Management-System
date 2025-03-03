﻿using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Shared.Infrastructure.Abstractions;
using Users.Application.Features.Managers.Interfaces;
using Users.Domain.Entities;
using Users.Domain.Responses;

namespace Users.Application.Features.Users.UpdateUser;

public sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand>
{
	private readonly IRepositoryManager _repositotyManager;
	private readonly IUnitOfWork _unitOfWork;
	public UpdateUserCommandHandler(IRepositoryManager repositotyManager, IUnitOfWork unitOfWork)
	{
		_repositotyManager = repositotyManager;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
	{
		var userResult = await _repositotyManager.User.GetByIdAsync(request.Id);
		if (userResult.IsFailure)
			return Result.Failure(userResult.Response);

		User user = userResult.Value!;

		if (!string.IsNullOrEmpty(request.NewEmail) && request.NewEmail != user.Email)
		{
			var emailCheckResult = await _repositotyManager.User.GetByEmailAsync(request.NewEmail);
			if (emailCheckResult.IsSuccess)
				return Result.Failure(Responses.EmailTaken);

			user.Email = request.NewEmail;
		}

		user.FirstName = request.FirstName ?? user.FirstName;
		user.LastName = request.LastName ?? user.LastName;

		_repositotyManager.User.UpdateAsync(user);

		await _unitOfWork.SaveChangesAsync();
		return Result.Success(Responses.UpdateSuccessful);
	}
}
