namespace Shared.Application.Helpers.Abstractions;

public interface IJwtParser
{
	string? GetIdFromToken();
}