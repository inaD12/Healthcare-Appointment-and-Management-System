using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Options;
using Serilog;
using System.Security.Claims;
using Users.Application.Factories;
using Users.Application.Settings;
using Users.Domain.DTOs.Responses;

namespace Users.Application.Auth.TokenManager
{
	public class TokenManager : ITokenManager
	{
		private readonly IOptionsMonitor<AuthValues> _jwtOptions;
		private readonly ITokenDTOFactory _entityFactory;

		public TokenManager(IOptionsMonitor<AuthValues> jwtOptions, ITokenDTOFactory entityFactory)
		{
			_jwtOptions = jwtOptions;
			_entityFactory = entityFactory;
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

				return _entityFactory.CreateToken(token);
			}
			catch (Exception ex)
			{
				Log.Error(ex.Message, ex);
			}

			return null;
		}
	}
}
