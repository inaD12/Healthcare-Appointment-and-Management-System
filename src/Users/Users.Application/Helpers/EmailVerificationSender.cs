using FluentEmail.Core;
using Serilog;
using Users.Application.EmailVerification;
using Users.Application.Factories;
using Users.Domain.EmailVerification;
using Users.Domain.Entities;
using Users.Domain.Result;
using Users.Infrastructure.DBContexts;

namespace Users.Application.Helpers
{
	internal class EmailVerificationSender : IEmailVerificationSender
	{
		private readonly IFluentEmail _fluentEmail;
		private readonly IEmailVerificationLinkFactory _emailVerificationLinkFactory;
		private readonly IEmailVerificationTokenFactory _emailVerificationTokenFactory;
		private readonly IDBContext _dbContext;

		public EmailVerificationSender(IFluentEmail fluentEmail,
			IEmailVerificationLinkFactory emailVerificationLinkFactory,
			IEmailVerificationTokenFactory emailVerificationTokenFactory,
			IDBContext dbContext)
		{
			_fluentEmail = fluentEmail;
			_emailVerificationLinkFactory = emailVerificationLinkFactory;
			_emailVerificationTokenFactory = emailVerificationTokenFactory;
			_dbContext = dbContext;
		}

		public async Task<Result> SendEmailAsync(User user)
		{
			try
			{
				DateTime utcNow = DateTime.UtcNow;

				EmailVerificationToken emailVerificationToken = _emailVerificationTokenFactory.CreateToken(
					Guid.NewGuid().ToString(),
					user.Id,
					utcNow,
					utcNow.AddDays(1));

				await _dbContext.EmailVerificationToken.AddTokenAsync(emailVerificationToken);

				string verificationLink = _emailVerificationLinkFactory.Create(emailVerificationToken);

				var response = await _fluentEmail
				   .To(user.Email)
				   .Subject("Email verifivation for HAMS")
				   .Body($"To verify your email <a href='{verificationLink}'>click here</a>", isHtml: true)
				   .SendAsync();

				return Result.Success();
			}
			catch (Exception ex)
			{
				Log.Error($"Error in SendEmail() in EmailVerificationSender: {ex.Message} {ex.StackTrace}");
				return Result.Failure(Response.InternalError);
			}
		}
	}
}
