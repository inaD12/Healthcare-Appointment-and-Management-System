using Microsoft.AspNetCore.Http;
using System.IdentityModel.Tokens.Jwt;

namespace Users.Application.Auth.Helpers;

public class JwtParser : IJwtParser
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public JwtParser(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public string GetIdFromToken()
	{
		return GetClaimValueFromToken("id");
	}

	private string GetClaimValueFromToken(string claimType)
	{
		var jwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

		if (string.IsNullOrEmpty(jwtToken))
		{
			return null;
		}

		var handler = new JwtSecurityTokenHandler();
		var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;

		string claimValue = jsonToken?.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
		return claimValue;
	}
}
