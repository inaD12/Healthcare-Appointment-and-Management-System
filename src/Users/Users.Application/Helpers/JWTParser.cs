using Microsoft.AspNetCore.Http;
using Serilog;
using System.IdentityModel.Tokens.Jwt;

namespace Users.Application.Helpers
{
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
			try
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
			catch (Exception ex)
			{
				Log.Error($"Error parsing token: {ex.Message}");
				return string.Empty;
			}
		}
	}
}
