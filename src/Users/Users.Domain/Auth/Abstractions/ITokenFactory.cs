using Shared.Domain.Enums;
using Users.Domain.Auth.Models;

namespace Users.Domain.Auth.Abstractions;

public interface ITokenFactory
{
	TokenResult CreateToken(string id, Roles role);
}