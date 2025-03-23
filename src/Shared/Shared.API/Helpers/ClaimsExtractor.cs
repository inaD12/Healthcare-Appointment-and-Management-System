using Microsoft.AspNetCore.Http;
using Shared.API.Abstractions;
using Shared.Utilities;
using System.Security.Claims;

namespace Shared.API.Helpers;

internal class ClaimsExtractor : IClaimsExtractor
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public ClaimsExtractor(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
	}

	public string GetUserId() => GetClaimValue(AppClaims.Id);

	private string GetClaimValue(string key)
	{
		var user = _httpContextAccessor.HttpContext?.User!;
		return user.FindFirstValue(key)!;
	}
}