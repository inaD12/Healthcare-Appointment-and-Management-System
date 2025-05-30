using Shared.Domain.Results;

namespace Shared.Application.Helpers.Abstractions;

public interface IJwtParser
{
	Result<string> GetIdFromToken();
}