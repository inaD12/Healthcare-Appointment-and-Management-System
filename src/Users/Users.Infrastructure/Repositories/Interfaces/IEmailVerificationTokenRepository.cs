using Users.Domain.EmailVerification;
using Users.Domain.Result;

namespace Users.Infrastructure.Repositories.Interfaces
{
    public interface IEmailVerificationTokenRepository
    {
        Task AddTokenAsync(EmailVerificationToken token);
        Task<Result<EmailVerificationToken>> GetTokenByIdAsync(string tokenId);
    }
}