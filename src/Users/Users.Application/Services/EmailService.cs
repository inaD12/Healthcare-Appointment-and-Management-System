using Users.Domain.Result;
using Users.Infrastructure.Repositories;

namespace Users.Application.Services
{
	public class EmailService : IEmailService
	{
		private readonly IEmailVerificationTokenRepository _emailVerificationTokenRepository;
		private readonly IUserRepository _userRepository;

		public EmailService(IEmailVerificationTokenRepository emailVerificationTokenRepository, IUserRepository userRepository)
		{
			_emailVerificationTokenRepository = emailVerificationTokenRepository;
			_userRepository = userRepository;
		}

		public async Task<Result> Handle(string tokenId)
		{
			var res = await _emailVerificationTokenRepository.GetTokenById(tokenId);

			if (res.IsFailure || res.Value.ExpiresOnUtc < DateTime.UtcNow || res.Value.User.EmailVerified)
			{
				return Result.Failure(Response.InvalidVerificationToken);
			}

			_userRepository.VerifyEmail(res.Value.User);

			return Result.Success(Response.Ok);
		}
	}
}
