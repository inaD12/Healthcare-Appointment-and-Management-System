using Users.Domain.Entities;

namespace Users.Application.Features.Email.Factories.Abstractions;

public interface IEmailVerificationLinkFactory
{
	string Create(EmailVerificationToken token);
}