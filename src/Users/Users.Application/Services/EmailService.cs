using Users.Domain.Result;
using Users.Infrastructure.DBContexts;

namespace Users.Application.Services
{
	public class EmailService : IEmailService
	{
		private readonly IDBContext _dbContext;

		public EmailService(IDBContext dbContext)
		{
			_dbContext = dbContext;
		}

		public async Task<Result> Handle(string tokenId)
		{
			var res = await _dbContext.EmailVerificationToken.GetTokenByIdAsync(tokenId);

			if (res.IsFailure || res.Value.ExpiresOnUtc < DateTime.UtcNow || res.Value.User.EmailVerified)
			{
				return Result.Failure(Response.InvalidVerificationToken);
			}

			await _dbContext.User.VerifyEmailAsync(res.Value.User);

			return Result.Success(Response.Ok);
		}
	}
}
