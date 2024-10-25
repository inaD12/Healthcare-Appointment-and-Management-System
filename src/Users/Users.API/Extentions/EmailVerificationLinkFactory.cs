using Healthcare_Appointment_and_Management_System.EndPoints;
using Users.Application.EmailVerification;
using Users.Domain.EmailVerification;

namespace Users.API.EmailVerification
{
	internal class EmailVerificationLinkFactory(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator) : IEmailVerificationLinkFactory
	{
		public string Create(EmailVerificationToken token)
		{
			string? verificationLink = linkGenerator.GetUriByName(
				httpContextAccessor.HttpContext!,
				UserEndPoints.VerifyEmail,
				new { token = token.Id });

			return verificationLink;
		}
	}
}
