using Shared.Application.Abstractions;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Results;
using Users.Application.Features.Users.Mappers;
using Users.Application.Features.Users.Models;
using Users.Domain.Abstractions.Repositories;
using Users.Domain.Utilities;

namespace Users.Application.Features.Users.Commands.UpdateUser;

public sealed class UpdateUserCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
	: ICommandHandler<UpdateUserCommand, UserCommandViewModel>
{
	public async Task<Result<UserCommandViewModel>> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
	{
		var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);
		if (user == null)
			return Result<UserCommandViewModel>.Failure(ResponseList.UserNotFound);

		user.UpdateProfile(request.FirstName, request.LastName);

		userRepository.Update(user);

		await unitOfWork.SaveChangesAsync(cancellationToken);
		var userCommandViewModel = user.ToCommandViewModel();
		return Result<UserCommandViewModel>.Success(userCommandViewModel, ResponseList.UpdateSuccessful);
	}
}
