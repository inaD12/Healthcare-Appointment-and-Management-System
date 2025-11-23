using Shared.Domain.Abstractions;
using Users.Domain.Entities;

namespace Users.Domain.Abstractions.Repositories;

public interface IEmailVerificationTokenRepository : IGenericRepository<EmailVerificationToken>
{
}