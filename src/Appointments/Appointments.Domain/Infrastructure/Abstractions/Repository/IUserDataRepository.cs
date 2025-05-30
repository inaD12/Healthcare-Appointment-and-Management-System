using Appointments.Domain.Entities;
using Shared.Domain.Abstractions;

namespace Appointments.Domain.Infrastructure.Abstractions.Repository;

public interface IUserDataRepository : IGenericRepository<UserData>
{
	Task<UserData?> GetUserDataByEmailAsync(string id);
}
