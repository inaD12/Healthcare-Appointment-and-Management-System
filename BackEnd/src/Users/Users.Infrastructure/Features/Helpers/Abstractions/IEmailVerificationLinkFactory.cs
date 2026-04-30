using Users.Domain.Entities;

namespace Users.Infrastructure.Features.Helpers.Abstractions;

public interface IEmailVerificationLinkFactory
{
	string Create(EmailVerificationToken token);
}