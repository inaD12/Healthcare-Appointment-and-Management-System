using Users.Application.Managers.Interfaces;
using Users.Application.Services.Interfaces;
using Users.Domain.Result;

namespace Users.Application.Services
{
    public class EmailService : IEmailService
	{
		private readonly IRepositoryManager _repositoryManager;

		public EmailService(IRepositoryManager repositotyManager)
		{
			_repositoryManager = repositotyManager;
		}

		public async Task<Result> HandleAsync(string tokenId)
		{
			var tokenResult = await _repositoryManager.EmailVerificationToken.GetTokenByIdAsync(tokenId);

			if (tokenResult.IsFailure ||
				tokenResult.Value.ExpiresOnUtc < DateTime.UtcNow ||
				tokenResult.Value.User.EmailVerified)
			{
				return Result.Failure(Response.InvalidVerificationToken);
			}

			await _repositoryManager.User.VerifyEmailAsync(tokenResult.Value.User);

			return Result.Success(Response.Ok);
		}
	}
}
