﻿using Microsoft.AspNetCore.Http;
using Serilog;
using Shared.Application.Helpers.Abstractions;
using Shared.Domain.Responses;
using Shared.Domain.Results;
using System.IdentityModel.Tokens.Jwt;

namespace Shared.Application.Helpers;

public class JwtParser : IJwtParser
{
	private readonly IHttpContextAccessor _httpContextAccessor;

	public JwtParser(IHttpContextAccessor httpContextAccessor)
	{
		_httpContextAccessor = httpContextAccessor;
	}

	public Result<string> GetIdFromToken()
	{
		return GetClaimValueFromToken("id");
	}

	private Result<string> GetClaimValueFromToken(string claimType)
	{
		try
		{
			var jwtToken = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

			if (string.IsNullOrEmpty(jwtToken))
			{
				return Result<string>.Failure(SharedResponses.JWTNotFound);
			}

			var handler = new JwtSecurityTokenHandler();
			var jsonToken = handler.ReadToken(jwtToken) as JwtSecurityToken;

			string claimValue = jsonToken?.Claims.FirstOrDefault(c => c.Type == claimType)?.Value;
			if (string.IsNullOrEmpty(claimValue))
			{
				Log.Error($"Token doesn't contain the requested claim type: {claimType}");
				return Result<string>.Failure(SharedResponses.InternalError);
			}

			return Result<string>.Success(claimValue);
		}
		catch (Exception ex)
		{
			Log.Error($"Error parsing token: {ex.Message}");
			return Result<string>.Failure(SharedResponses.InternalError);
		}
	}
}
