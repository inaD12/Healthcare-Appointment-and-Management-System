using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.Domain.Enums;
using Shared.Domain.Options;
using Shared.Infrastructure.Clock;
using Shared.Utilities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Users.Domain.Auth.Abstractions;
using Users.Domain.Auth.Models;

namespace Users.Infrastructure.Features.Auth;

public class TokenFactory : ITokenFactory
{
	private readonly AuthOptions _jwtOptions;
	private readonly IDateTimeProvider _dateTimeProvider;

	public TokenFactory(IOptionsMonitor<AuthOptions> jwtOptions, IDateTimeProvider dateTimeProvider)
	{
		_jwtOptions = jwtOptions.CurrentValue;
		_dateTimeProvider = dateTimeProvider;
	}

	public TokenResult CreateToken(string id, Roles role)
	{
		byte[] secretKeyBytes = Encoding.UTF8.GetBytes(_jwtOptions.SecretKey);
		var signingKey = new SymmetricSecurityKey(secretKeyBytes);
		var expiration = _dateTimeProvider.GetUtcNow(_jwtOptions.SecondsValid);

		var claimsIdentity = new ClaimsIdentity(
		[
			new Claim(AppClaims.Id, id),
			new Claim(AppClaims.Role, role.ToString())
		]);


		var tokenDescriptor = new SecurityTokenDescriptor
		{
			Subject = claimsIdentity,
			SigningCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha512Signature),
			Issuer = _jwtOptions.Issuer,
			Audience = _jwtOptions.Audience,
			Expires = expiration
		};

		var tokenHandler = new JwtSecurityTokenHandler();
		var securityToken = tokenHandler.CreateToken(tokenDescriptor);
		var token = tokenHandler.WriteToken(securityToken);

		return new TokenResult(token);
	}
}
