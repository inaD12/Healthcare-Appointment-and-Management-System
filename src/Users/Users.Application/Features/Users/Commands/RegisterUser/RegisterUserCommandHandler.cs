using Shared.Application.Abstractions;
using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Users.Application.Features.Users.Models;
using Users.Domain.Auth.Abstractions;
using Users.Domain.Auth.Models;
using Users.Domain.Entities;
using Users.Domain.Infrastructure.Abstractions.Repositories;
using Users.Domain.Responses;

namespace Users.Application.Features.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, UserCommandViewModel>
{
	private readonly IUserRepository _userRepository;
	private readonly IPasswordManager _passwordManager;
	private readonly IHAMSMapper _hamsMapper;
	private readonly IUnitOfWork _unitOfWork;

	public RegisterUserCommandHandler(IPasswordManager passwordManager, IHAMSMapper hamsMapper, IUnitOfWork unitOfWork, IUserRepository userRepository)
	{
		_passwordManager = passwordManager;
		_hamsMapper = hamsMapper;
		_unitOfWork = unitOfWork;
		_userRepository = userRepository;
	}

	public async Task<Result<UserCommandViewModel>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		var emailUser = await _userRepository.GetByEmailAsync(request.Email);
		if (emailUser != null)
			return Result<UserCommandViewModel>.Failure(ResponseList.EmailTaken);

		PasswordHashResult passwordHashResult = _passwordManager.HashPassword(request.Password);
		var user = User.Create(request.Email, passwordHashResult.PasswordHash, passwordHashResult.Salt, request.Role, request.FirstName, request.LastName, request.DateOfBirth, request.PhoneNumber, request.Address);
		await _userRepository.AddAsync(user);
		await _unitOfWork.SaveChangesAsync();

		var userCommandViewModel = _hamsMapper.Map<UserCommandViewModel>(user);
		return Result<UserCommandViewModel>.Success(userCommandViewModel, ResponseList.RegistrationSuccessful);
	}
}
