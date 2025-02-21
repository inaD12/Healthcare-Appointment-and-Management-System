using Users.Domain.Abstractions.Repositories;

namespace Users.Application.Features.Managers.Interfaces;

public interface IRepositoryManager
{
	IUserRepository User { get; }
	IEmailVerificationTokenRepository EmailVerificationToken { get; }
}