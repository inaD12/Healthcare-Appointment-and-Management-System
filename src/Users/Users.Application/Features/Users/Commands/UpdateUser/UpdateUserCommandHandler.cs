using Shared.Application.Abstractions;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Users.Application.Features.Users.Models;
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
		var user = await _userRepository.GetByIdAsync(request.Id);
		if (user == null)
			return Result<UserCommandViewModel>.Failure(ResponseList.UserNotFound);

		if (!string.IsNullOrEmpty(request.NewEmail) && request.NewEmail != user.Email)
		{
			var emailCheck = await _userRepository.GetByEmailAsync(request.NewEmail);
			if (emailCheck != null)
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
