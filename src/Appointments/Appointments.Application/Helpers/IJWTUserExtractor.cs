using Shared.Domain.Results;

namespace Appointments.Application.Helpers;

public interface IJWTUserExtractor
{
	Task<Result<string>> GetUserIdFromTokenAsync();
}
