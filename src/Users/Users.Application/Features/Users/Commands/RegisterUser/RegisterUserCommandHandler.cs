using Shared.Application.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Events;
using Shared.Domain.Results;
using Shared.Infrastructure.Abstractions;
using Users.Application.Features.Auth.Abstractions;
using Users.Application.Features.Auth.Models;
using Users.Application.Features.Email.Helpers.Abstractions;
using Users.Application.Features.Email.Models;
using Users.Application.Features.Users.Models;
using Users.Domain.Entities;
using Users.Domain.Infrastructure.Abstractions.Repositories;
using Users.Domain.Responses;

namespace Users.Application.Features.Users.Commands.RegisterUser;

public sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand, UserCommandViewModel>
{
	private readonly IUserRepository _userRepository;
	private readonly IPasswordManager _passwordManager;
	private readonly IEventBus _eventBus;
	private readonly IEmailConfirmationTokenPublisher _emailConfirmationTokenPublisher;
	private readonly IHAMSMapper _hamsMapper;
	private readonly IUnitOfWork _unitOfWork;

	public RegisterUserCommandHandler(IPasswordManager passwordManager, IEventBus eventBus, IEmailConfirmationTokenPublisher emailConfirmationTokenPublisher, IHAMSMapper hamsMapper, IUnitOfWork unitOfWork, IUserRepository userRepository)
	{
		_passwordManager = passwordManager;
		_eventBus = eventBus;
		_emailConfirmationTokenPublisher = emailConfirmationTokenPublisher;
		_hamsMapper = hamsMapper;
		_unitOfWork = unitOfWork;
		_userRepository = userRepository;
	}

	public async Task<Result<UserCommandViewModel>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
	{
		var res = await _userRepository.GetByEmailAsync(request.Email);
		if (res.IsSuccess)
			return Result<UserCommandViewModel>.Failure(Responses.EmailTaken);

		PasswordHashResult passwordHashResult = _passwordManager.HashPassword(request.Password);
		User user = _hamsMapper.Map<User>((passwordHashResult,request));
		await _userRepository.AddAsync(user);

		var userCreatedEvent = _hamsMapper.Map<UserCreatedEvent>(user);
		await _eventBus.PublishAsync(userCreatedEvent);

		var publishEmailConfirmationTokenModel = _hamsMapper.Map<PublishEmailConfirmationTokenModel>(user);
		await _emailConfirmationTokenPublisher.PublishEmailConfirmationTokenAsync(publishEmailConfirmationTokenModel);

		await _unitOfWork.SaveChangesAsync();
		var userCommandViewModel = _hamsMapper.Map<UserCommandViewModel>(user);
		return Result<UserCommandViewModel>.Success(userCommandViewModel, Responses.RegistrationSuccessful);
	}
}
