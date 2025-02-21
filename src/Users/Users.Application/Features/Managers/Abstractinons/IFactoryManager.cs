using Users.Application.Features.Email.Factories.Abstractions;
using Users.Application.Features.Users.Factories.Abstractions;

namespace Users.Application.Features.Managers.Interfaces;

public interface IFactoryManager
{
	IEmailVerificationTokenFactory EmailTokenFactory { get; }
	IMessageDTOFactory MessageDTOFactory { get; }
	ITokenDTOFactory TokenDTOFactory { get; }
	IUserFactory UserFactory { get; }
	IEmailVerificationLinkFactory EmailLinkFactory { get; }
	IUserCreatedEventFactory UserCreatedEventFactory { get; }
	IUserConfirmEmailEventFactory UserConfirmEmailEventFactory { get; }
}