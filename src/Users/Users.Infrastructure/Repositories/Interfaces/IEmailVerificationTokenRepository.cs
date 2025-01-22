using Contracts.Results;
using Users.Domain.EmailVerification;

namespace Users.Infrastructure.Repositories.Interfaces
{
    public interface IEmailVerificationTokenRepository
    {
        Task AddTokenAsync(EmailVerificationToken token);
        Task<Result<EmailVerificationToken>> GetTokenByIdAsync(string tokenId);
    }
}