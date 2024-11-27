using Appointments.Domain.Result;

namespace Appointments.Application.Helpers
{
	public interface IJwtParser
	{
		Result<string> GetIdFromToken();
	}
}