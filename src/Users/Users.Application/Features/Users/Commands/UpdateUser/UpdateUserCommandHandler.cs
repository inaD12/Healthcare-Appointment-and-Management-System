using Shared.Application.Abstractions;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Users.Application.Features.Users.Models;
using Users.Domain.Entities;
using Users.Domain.Infrastructure.Abstractions.Repositories;
using Users.Domain.Responses;

namespace Users.Application.Features.Users.UpdateUser;

public sealed class UpdateUserCommandHandler : ICommandHandler<UpdateUserCommand, UserCommandViewModel>
{
	private readonly IUserRepository _userRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IHAMSMapper _mapper;
	public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IHAMSMapper mapper, IUserRepository userRepository)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_userRepository = userRepository;
	}

	public async Task<Result<UserCommandViewModel>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
	{
		var userResult = await _userRepository.GetByIdAsync(request.Id);
		if (userResult.IsFailure)
			return Result<UserCommandViewModel>.Failure(userResult.Response);

		User user = userResult.Value!;

		if (!string.IsNullOrEmpty(request.NewEmail) && request.NewEmail != user.Email)
		{
			var emailCheckResult = await _userRepository.GetByEmailAsync(request.NewEmail);
			if (emailCheckResult.IsSuccess)
				return Result<UserCommandViewModel>.Failure(ResponseList.EmailTaken);

			user.UpdateProfile(request.NewEmail, request.FirstName, request.LastName);
		}
		else
		{
			user.UpdateProfile(null, request.FirstName, request.LastName);
		}

		_userRepository.Update(user);

		await _unitOfWork.SaveChangesAsync();
		var userCommandViewModel = _mapper.Map<UserCommandViewModel>(user);
		return Result<UserCommandViewModel>.Success(userCommandViewModel, ResponseList.UpdateSuccessful);
	}
}
