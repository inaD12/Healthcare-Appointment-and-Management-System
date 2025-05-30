using Appointments.Domain.Entities;
using Shared.Domain.Abstractions;
using Shared.Domain.Results;

namespace Appointments.Domain.Abstractions.Repository;

public interface IUserDataRepository : IGenericRepository<UserData>
{
	Task<Result<UserData>> GetUserDataByEmailAsync(string id);
}
