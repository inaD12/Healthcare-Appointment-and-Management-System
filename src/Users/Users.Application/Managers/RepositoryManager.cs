using Microsoft.Extensions.DependencyInjection;
using Users.Application.Managers.Interfaces;
using Users.Infrastructure.Repositories.Interfaces;

namespace Users.Application.Managers
{
    internal class RepositoryManager : IRepositoryManager
    {
        private readonly IServiceProvider _serviceProvider;

        public RepositoryManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IUserRepository User => _serviceProvider.GetRequiredService<IUserRepository>();
        public IEmailVerificationTokenRepository EmailVerificationToken => _serviceProvider.GetRequiredService<IEmailVerificationTokenRepository>();
    }
}
