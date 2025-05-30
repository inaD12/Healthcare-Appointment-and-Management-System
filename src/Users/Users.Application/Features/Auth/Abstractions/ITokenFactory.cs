using Users.Application.Features.Auth.Models;

namespace Users.Application.Features.Auth.Abstractions;

public interface ITokenFactory
{
	TokenResult CreateToken(string id);
}