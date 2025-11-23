using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Users.Application.Features.Users.Mappers;
using Users.Application.Features.Users.Models;
using Users.Domain.Abstractions.Repositories;
using Users.Domain.Auth.Abstractions;
using Users.Domain.Auth.Models;
using Users.Domain.Entities;
using Users.Domain.Utilities;

namespace Users.Application.Features.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler(
	IUnitOfWork unitOfWork,
	IUserRepository userRepository,
	IIdentityProviderService identityProviderService)
	: ICommandHandler<RegisterUserCommand, UserCommandViewModel>
{
	public async Task<Result<UserCommandViewModel>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		Result<string> identityResult = await identityProviderService.RegisterUserAsync(
			new UserModel(request.Email, request.Password, request.FirstName, request.LastName),
			cancellationToken);

		if (identityResult.IsFailure)
			return Result<UserCommandViewModel>.Failure(identityResult.Response);

		var user = User.Create(request.Email, request.Role, request.FirstName, request.LastName,  request.DateOfBirth, identityResult.Value!, request.PhoneNumber, request.Address);
		await userRepository.AddAsync(user, cancellationToken);
		await unitOfWork.SaveChangesAsync(cancellationToken);

		var userCommandViewModel = user.ToCommandViewModel();
		return Result<UserCommandViewModel>.Success(userCommandViewModel, ResponseList.RegistrationSuccessful);
	}
}
