using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Options;
using Shared.Domain.Options;
using Users.Application.Factories.Interfaces;
using Users.Domain.DTOs.Responses;

namespace Users.Application.Auth.TokenManager;

public class TokenManager : ITokenManager
{
	private readonly AuthOptions _jwtOptions;
	private readonly ITokenDTOFactory _tokenDTOfactory;

	public TokenManager(IOptionsMonitor<AuthOptions> jwtOptions, ITokenDTOFactory tokenDTOfactory)
	{
		_jwtOptions = jwtOptions.CurrentValue;
		_tokenDTOfactory = tokenDTOfactory;
	}

	public TokenDTO CreateToken(string id)
	{
		string token = JwtBuilder.Create()
			.WithAlgorithm(new HMACSHA256Algorithm())
			.WithSecret(_jwtOptions.SecretKey)
			.AddClaim("id", id)
			//.AddClaim(ClaimTypes.Role, role)
			.AddClaim("exp", DateTimeOffset.UtcNow.AddSeconds(_jwtOptions.SecondsValid).ToUnixTimeSeconds())
			.AddClaim("iss", _jwtOptions.Issuer)
			.AddClaim("aud", _jwtOptions.Audience)
			.Encode();

		return _tokenDTOfactory.CreateToken(token);
	}
}
