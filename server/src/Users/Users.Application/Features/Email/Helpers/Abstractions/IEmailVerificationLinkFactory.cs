using Users.Domain.Entities;

namespace Users.Application.Features.Email.Helpers.Abstractions;

public interface IEmailVerificationLinkFactory
{
	string Create(EmailVerificationToken token);
}