using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Options;
using Serilog;
using Users.Application.Factories.Interfaces;
using Users.Application.Settings;
using Users.Domain.DTOs.Responses;

namespace Users.Application.Auth.TokenManager
{
	public class TokenManager : ITokenManager
	{
		private readonly IOptionsMonitor<AuthValues> _jwtOptions;
		private readonly ITokenDTOFactory _tokenDTOfactory;

		public TokenManager(IOptionsMonitor<AuthValues> jwtOptions, ITokenDTOFactory tokenDTOfactory)
		{
			_jwtOptions = jwtOptions;
			_tokenDTOfactory = tokenDTOfactory;
		}

		public TokenDTO CreateToken(string id)
		{
			try
			{
				string token = JwtBuilder.Create()
					.WithAlgorithm(new HMACSHA256Algorithm())
					.WithSecret(_jwtOptions.CurrentValue.SecretKey)
					.AddClaim("id", id)
					//.AddClaim(ClaimTypes.Role, role)
					.AddClaim("exp", DateTimeOffset.UtcNow.AddSeconds(_jwtOptions.CurrentValue.SecondsValid).ToUnixTimeSeconds())
					.AddClaim("iss", _jwtOptions.CurrentValue.Issuer)
					.AddClaim("aud", _jwtOptions.CurrentValue.Audience)
					.Encode();

				return _tokenDTOfactory.CreateToken(token);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in CreateToken() in TokenManager: {ex.Message} {ex.Source} {ex.InnerException}");
				return null;
			}
		}
	}
}
