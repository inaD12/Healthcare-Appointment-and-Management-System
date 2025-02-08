using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Users.Application.Factories.Interfaces;
using Users.Domain.EmailVerification;

namespace Users.Application.Factories;

internal class EmailVerificationLinkFactory(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator) : IEmailVerificationLinkFactory
{
	public string Create(EmailVerificationToken token)
	{
		string? verificationLink = linkGenerator.GetUriByName(
			httpContextAccessor.HttpContext!,
			"VerifyEmail",
			new { token = token.Id });

		return verificationLink;
	}
}
