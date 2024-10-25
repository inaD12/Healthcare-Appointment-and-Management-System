using Users.Domain.EmailVerification;

namespace Users.Application.EmailVerification
{
	public interface IEmailVerificationLinkFactory
	{
		string Create(EmailVerificationToken token);
	}
}