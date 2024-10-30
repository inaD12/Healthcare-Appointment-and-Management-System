using Users.Application.Factories.Interfaces;

namespace Users.Application.Managers.Interfaces
{
    public interface IFactoryManager
    {
        IEmailVerificationTokenFactory EmailTokenFactory { get; }
        IMessageDTOFactory MessageDTOFactory { get; }
        ITokenDTOFactory TokenDTOFactory { get; }
        IUserFactory UserFactory { get; }
        IEmailVerificationLinkFactory EmailLinkFactory { get; }

    }
}