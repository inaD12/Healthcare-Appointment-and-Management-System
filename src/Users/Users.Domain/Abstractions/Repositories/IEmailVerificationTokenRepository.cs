using Shared.Domain.Abstractions;
using Users.Domain.EmailVerification;

namespace Users.Domain.Abstractions.Repositories;

public interface IEmailVerificationTokenRepository : IGenericRepository<EmailVerificationToken>
{
}