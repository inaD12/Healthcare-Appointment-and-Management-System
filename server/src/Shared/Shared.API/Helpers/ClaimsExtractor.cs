using Microsoft.AspNetCore.Http;
using Shared.API.Abstractions;
using Shared.API.Models;
using Shared.Utilities;
using System.IdentityModel.Tokens.Jwt;
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

	public string GetUserRole() => GetClaimValue(AppClaims.Role);

	private string GetClaimValue(string key)
	{
		var user = _httpContextAccessor.HttpContext?.User!;
		return user.FindFirstValue(key)!;
	}

	public ClaimsExtractorModel GetAllClaims()
	{
		var user = _httpContextAccessor.HttpContext?.User;

		var claimsDictionary = user!.Claims
								   .ToDictionary(c => c.Type, c => c.Value);

		return new ClaimsExtractorModel(claimsDictionary);
	}

	private string GetClaimFromToken(string token, string key)
	{
		var handler = new JwtSecurityTokenHandler();

		if (!handler.CanReadToken(token))
		{
			throw new ArgumentException("Invalid JWT token.", nameof(token));
		}

		var jwtToken = handler.ReadJwtToken(token);
		var claimValue = jwtToken.Claims.FirstOrDefault(c => c.Type == key)?.Value;

		return claimValue ?? throw new KeyNotFoundException($"Claim '{key}' not found in token.");
	}

	public string GetUserId(string Token) => GetClaimFromToken(Token, AppClaims.Id);

	public string GetUserRole(string Token) => GetClaimFromToken(Token,AppClaims.Role);
}