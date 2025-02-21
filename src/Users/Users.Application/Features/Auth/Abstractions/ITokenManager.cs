using Users.Domain.DTOs.Responses;

namespace Users.Application.Features.Auth.Abstractions;

public interface ITokenManager
{
	TokenDTO CreateToken(string id);
}