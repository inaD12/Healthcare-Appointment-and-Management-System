using Shared.Application.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Shared.Infrastructure.Abstractions;
using Users.Application.Features.Managers.Interfaces;
using Users.Application.Features.Users.Models;
using Users.Domain.Entities;
using Users.Domain.Responses;

namespace Users.Application.Features.Users.UpdateUser;

public sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UserCommandViewModel>
{
	private readonly IRepositoryManager _repositotyManager;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IHAMSMapper _mapper;
	public UpdateUserCommandHandler(IRepositoryManager repositotyManager, IUnitOfWork unitOfWork, IHAMSMapper mapper)
	{
		_repositotyManager = repositotyManager;
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

	public async Task<Result<UserCommandViewModel>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
	{
		var userResult = await _repositotyManager.User.GetByIdAsync(request.Id);
		if (userResult.IsFailure)
			return Result<UserCommandViewModel>.Failure(userResult.Response);

		User user = userResult.Value!;

		if (!string.IsNullOrEmpty(request.NewEmail) && request.NewEmail != user.Email)
		{
			var emailCheckResult = await _repositotyManager.User.GetByEmailAsync(request.NewEmail);
			if (emailCheckResult.IsSuccess)
				return Result<UserCommandViewModel>.Failure(Responses.EmailTaken);

			user.Email = request.NewEmail;
		}

		user.FirstName = request.FirstName ?? user.FirstName;
		user.LastName = request.LastName ?? user.LastName;

		_repositotyManager.User.UpdateAsync(user);

		await _unitOfWork.SaveChangesAsync();
		var userCommandViewModel = _mapper.Map<UserCommandViewModel>(user);
		return Result<UserCommandViewModel>.Success(userCommandViewModel, Responses.UpdateSuccessful);
	}
}
