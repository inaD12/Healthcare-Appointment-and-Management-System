using JWT.Algorithms;
using JWT.Builder;
using Microsoft.Extensions.Options;
using Shared.Domain.Options;
using Users.Application.Features.Auth.Abstractions;
using Users.Application.Features.Auth.Models;

namespace Users.Application.Features.Auth;

public class TokenFactory : ITokenFactory
{
	private readonly AuthOptions _jwtOptions;

	public TokenFactory(IOptionsMonitor<AuthOptions> jwtOptions)
	{
		_jwtOptions = jwtOptions.CurrentValue;
	}

	public TokenResult CreateToken(string id)
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

		return new TokenResult(token);
	}
}
