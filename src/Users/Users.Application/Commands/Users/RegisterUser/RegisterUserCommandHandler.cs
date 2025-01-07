using Contracts.Abstractions.Messaging;
using Contracts.Results;
using Users.Application.Auth.PasswordManager;
using Users.Application.Helpers.Interfaces;
using Users.Application.Managers.Interfaces;
using Users.Domain.Entities;
using Users.Domain.Responses;
using Users.Infrastructure.MessageBroker;

namespace Users.Application.Commands.Users.RegisterUser
{
	internal sealed class RegisterUserCommandHandler : ICommandHandler<RegisterUserCommand>
	{
		private readonly IRepositoryManager _repositotyManager;
		private readonly IEmailVerificationSender _emailVerificationSender;
		private readonly IFactoryManager _factoryManager;
		private readonly IPasswordManager _passwordManager;
		private readonly IEventBus _eventBus;

		public RegisterUserCommandHandler(IRepositoryManager repositotyManager, IEmailVerificationSender emailVerificationSender, IFactoryManager factoryManager, IPasswordManager passwordManager, IEventBus eventBus)
		{
			_repositotyManager = repositotyManager;
			_emailVerificationSender = emailVerificationSender;
			_factoryManager = factoryManager;
			_passwordManager = passwordManager;
			_eventBus = eventBus;
		}

		public async Task<Result> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
		{
			var res = await _repositotyManager.User.GetUserByEmailAsync(request.Email);

			if (res.IsSuccess)
				return Result.Failure(Responses.EmailTaken);

			User user = _factoryManager.UserFactory.CreateUser(
				request.Email,
				_passwordManager.HashPassword(request.Password, out string salt),
				salt,
				request.FirstName,
				request.LastName,
				request.DateOfBirth,
				request.PhoneNumber,
				request.Address,
				request.Role
				);

			await _repositotyManager.User.AddUserAsync(user);

			await _eventBus.PublishAsync(
				_factoryManager.UserCreatedEventFactory.CreateUserCreatedEvent(
					user.Id,
					user.Email,
					user.Role
				));

			await _repositotyManager.User.SaveChangesAsync();

			//Result emailSenderResult = await _emailVerificationSender.SendEmailAsync(user);

			//if (emailSenderResult.IsFailure)
			//	return emailSenderResult;

			return Result.Success(Responses.RegistrationSuccessful);
		}
	}
}
