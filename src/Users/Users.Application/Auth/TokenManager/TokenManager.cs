using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Options;
using Serilog;
using System.Security.Claims;
using Users.Application.Settings;

namespace Users.Application.Auth.TokenManager
{
	public class TokenManager : ITokenManager
	{
		private readonly IOptionsMonitor<AuthValues> _jwtOptions;

		public TokenManager(IOptionsMonitor<AuthValues> jwtOptions)
		{
			_jwtOptions = jwtOptions;
		}

		public string CreateToken(string username, string role, int secondsValid)
		{
			try
			{
				string token = JwtBuilder.Create()
					.WithAlgorithm(new HMACSHA256Algorithm())
					.WithSecret(_jwtOptions.CurrentValue.SecretKey)
					.AddClaim("username", username)
					.AddClaim(ClaimTypes.Role, role)
					.AddClaim("exp", DateTimeOffset.UtcNow.AddSeconds(secondsValid).ToUnixTimeSeconds())
					.AddClaim("iss", _jwtOptions.CurrentValue.Issuer)
					.AddClaim("aud", _jwtOptions.CurrentValue.Audience)
					.Encode();

				return token;
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message, ex);
			}

			return null;
		}
	}
}
