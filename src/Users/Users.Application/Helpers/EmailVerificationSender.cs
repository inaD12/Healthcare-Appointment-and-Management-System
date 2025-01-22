using Contracts.Results;
using FluentEmail.Core;
using Serilog;
using Users.Application.Helpers.Interfaces;
using Users.Application.Managers.Interfaces;
using Users.Domain.EmailVerification;
using Users.Domain.Entities;
using Users.Domain.Responses;

namespace Users.Application.Helpers
{
    public class EmailVerificationSender : IEmailVerificationSender
	{
		private readonly IFluentEmail _fluentEmail;
		private readonly IFactoryManager _factoryManager;
		private readonly IRepositoryManager _repositoryManager;

		public EmailVerificationSender(IFluentEmail fluentEmail,
			IRepositoryManager repositotyManager,
			IFactoryManager factoryManager)
		{
			_fluentEmail = fluentEmail;
			_repositoryManager = repositotyManager;
			_factoryManager = factoryManager;
		}

		public async Task<Result> SendEmailAsync(User user)
		{
			try
			{
				DateTime utcNow = DateTime.UtcNow;

				EmailVerificationToken emailVerificationToken = _factoryManager.EmailTokenFactory.CreateToken(
					Guid.NewGuid().ToString(),
					user.Id,
					utcNow,
					utcNow.AddDays(1));


				string verificationLink = _factoryManager.EmailLinkFactory.Create(emailVerificationToken);

				var response = await _fluentEmail
				   .To(user.Email)
				   .Subject("Email verifivation for HAMS")
				   .Body($"To verify your email <a href='{verificationLink}'>click here</a>", isHtml: true)
				   .SendAsync();

				if (!response.Successful)
				{
					return Result.Failure(Responses.EmailNotSent);
				}

				await _repositoryManager.EmailVerificationToken.AddTokenAsync(emailVerificationToken);

				return Result.Success();
			}
			catch (Exception ex)
			{
				Log.Error($"Error in SendEmail() in EmailVerificationSender: {ex.Message} {ex.StackTrace}");
				return Result.Failure(Responses.InternalError);
			}
		}
	}
}
