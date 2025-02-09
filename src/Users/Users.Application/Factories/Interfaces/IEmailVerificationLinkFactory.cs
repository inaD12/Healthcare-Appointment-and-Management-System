using Users.Domain.Entities;

namespace Users.Application.Factories.Interfaces;

public interface IEmailVerificationLinkFactory
{
	string Create(EmailVerificationToken token);
}