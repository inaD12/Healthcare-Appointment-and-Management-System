using Users.Domain.EmailVerification;
using Users.Domain.Result;

namespace Users.Infrastructure.Repositories
{
	public interface IEmailVerificationTokenRepository
	{
		Task AddToken(EmailVerificationToken token);
		Task<Result<EmailVerificationToken>> GetTokenById(string tokenId);
	}
}