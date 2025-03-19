using Microsoft.AspNetCore.Http;
using Serilog;
using Shared.Application.Helpers.Abstractions;
using System.IdentityModel.Tokens.Jwt;

namespace Shared.Application.Helpers;

public class JwtParser : IJwtParser
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public JwtParser(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public string? GetIdFromToken()
	{
		return GetClaimValueFromToken("id");
	}

	private string? GetClaimValueFromToken(string claimType)
	{
			var jwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

			if (string.IsNullOrEmpty(jwtToken))
				return null;

			var handler = new JwtSecurityTokenHandler();
			var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;

			string claimValue = jsonToken?.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
			if (string.IsNullOrEmpty(claimValue))
			{
				Log.Error($"Token doesn't contain the requested claim type: {claimType}");
				return null;
			}

			return claimValue;
	}
}
