using Users.Domain.EmailVerification;

namespace Users.Application.Factories.Interfaces
{
    public interface IEmailVerificationLinkFactory
    {
        string Create(EmailVerificationToken token);
    }
}