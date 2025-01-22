using Users.Infrastructure.Repositories.Interfaces;

namespace Users.Application.Managers.Interfaces
{
    public interface IRepositoryManager
    {
        IUserRepository User { get; }
        IEmailVerificationTokenRepository EmailVerificationToken { get; }
    }
}