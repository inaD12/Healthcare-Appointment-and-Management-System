using Microsoft.Extensions.DependencyInjection;
using Users.Application.Factories.Interfaces;
using Users.Application.Managers.Interfaces;

namespace Users.Application.Managers
{
    internal class FactoryManager : IFactoryManager
    {
        private readonly IServiceProvider _serviceProvider;

        public FactoryManager(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public IUserFactory UserFactory => _serviceProvider.GetRequiredService<IUserFactory>();
        public IMessageDTOFactory MessageDTOFactory => _serviceProvider.GetRequiredService<IMessageDTOFactory>();
        public IEmailVerificationTokenFactory EmailTokenFactory => _serviceProvider.GetRequiredService<IEmailVerificationTokenFactory>();
        public ITokenDTOFactory TokenDTOFactory => _serviceProvider.GetRequiredService<ITokenDTOFactory>();
        public IEmailVerificationLinkFactory EmailLinkFactory => _serviceProvider.GetRequiredService<IEmailVerificationLinkFactory>();
		public IUserCreatedEventFactory UserCreatedEventFactory => _serviceProvider.GetRequiredService<IUserCreatedEventFactory>();
	}
}
