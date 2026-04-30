using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Users.Domain.Entities;
using Users.Infrastructure.Features.Helpers.Abstractions;

namespace Users.Infrastructure.Features.Helpers;

internal class EmailVerificationLinkFactory(IHttpContextAccessor httpContextAccessor, LinkGenerator linkGenerator) : IEmailVerificationLinkFactory
{
	public string Create(EmailVerificationToken token)
	{
		string? verificationLink = linkGenerator.GetUriByName(
			httpContextAccessor.HttpContext!,
			"VerifyEmail",
			new { token = token.Id });

		if (verificationLink == null)
			throw new InvalidOperationException("Failed to create email verification link.");

		return verificationLink;
	}
}
