using Contracts.Results;
using Users.Domain.Entities;

namespace Users.Infrastructure.Repositories.Interfaces
{
    public interface IUserRepository
    {
        Task<Result<IEnumerable<User>>> GetAllUsersAsync();
        Task<Result<User>> GetUserByIdAsync(string id);
        Task<Result<User>> GetUserByEmailAsync(string email);
        Task<Result<User>> GetUserByFirstNameAsync(string firstName);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(string id);
        Task VerifyEmailAsync(User user);
    }

}
