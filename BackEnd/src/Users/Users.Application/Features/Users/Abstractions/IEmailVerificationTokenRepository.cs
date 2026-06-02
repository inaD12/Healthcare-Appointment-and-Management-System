using Shared.Application.Abstractions;
using Users.Domain.Entities;

namespace Users.Application.Features.Users.Abstractions;

public interface IEmailVerificationTokenRepository : IGenericRepository<EmailVerificationToken>
{
}