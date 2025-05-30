using Users.Application.Features.Email.Helpers.Abstractions;

namespace Users.Application.Features.Managers.Interfaces;

public interface IFactoryManager
{
	IEmailVerificationTokenFactory EmailTokenFactory { get; }
	IEmailVerificationLinkFactory EmailLinkFactory { get; }
}