using Contracts.Abstractions.Messaging;
using Contracts.Results;
using FluentEmail.Core;
using Users.Application.Managers.Interfaces;
using Users.Domain.EmailVerification;
using Users.Domain.Responses;

namespace Users.Application.Commands.Email.SendEmail;

public sealed class SendEmailCommandHandler : ICommandHandler<SendEmailCommand>
{
	private readonly IFluentEmail _fluentEmail;
	private readonly IFactoryManager _factoryManager;
	private readonly IRepositoryManager _repositoryManager;

	public SendEmailCommandHandler(IFluentEmail fluentEmail, IFactoryManager factoryManager, IRepositoryManager repositoryManager)
	{
		_fluentEmail = fluentEmail;
		_factoryManager = factoryManager;
		_repositoryManager = repositoryManager;
	}

	public async Task<Result> Handle(SendEmailCommand request, CancellationToken cancellationToken)
	{
		DateTime utcNow = DateTime.UtcNow;

		EmailVerificationToken emailVerificationToken = _factoryManager.EmailTokenFactory.CreateToken(
			Guid.NewGuid().ToString(),
			request.userId,
			utcNow,
			utcNow.AddDays(1));


		string verificationLink = _factoryManager.EmailLinkFactory.Create(emailVerificationToken);

		var response = await _fluentEmail
		   .To(request.userEmail)
		   .Subject("Email verifivation for HAMS")
		   .Body($"To verify your email <a href='{verificationLink}'>click here</a>", isHtml: true)
		   .SendAsync();

		if (!response.Successful)
			return Result.Failure(Responses.EmailNotSent);

		await _repositoryManager.EmailVerificationToken.AddTokenAsync(emailVerificationToken);

		return Result.Success();
	}
}
