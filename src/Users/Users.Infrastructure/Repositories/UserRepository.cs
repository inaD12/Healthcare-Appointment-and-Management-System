using Contracts.Results;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Users.Domain.Entities;
using Users.Domain.Responses;
using Users.Infrastructure.Repositories.Interfaces;
using Users.Infrastructure.UsersDBContexts;

namespace Users.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
	{
		private readonly UsersDBContext _context;

		public UserRepository(UsersDBContext context)
		{
			_context = context;
		}

		public async Task<Result<IEnumerable<User>>> GetAllUsersAsync()
		{
			try
			{
				var users = await _context.Users.ToListAsync();

				if (!users.Any())
				{
					return Result<IEnumerable<User>>.Failure(Responses.NoUsersFound);
				}

				return Result<IEnumerable<User>>.Success(users);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in GetAllUsersAsync() in UserRepository: {ex.Message}");
				return Result<IEnumerable<User>>.Failure(Responses.InternalError);
			}
		}

		public async Task<Result<User>> GetUserByIdAsync(string id)
		{
			try
			{
				var user = await _context.Users.FindAsync(id);

				if (user == null)
				{
					return Result<User>.Failure(Responses.UserNotFound);
				}

				return Result<User>.Success(user);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in GetUserByIdAsync() in UserRepository: {ex.Message}");
				return Result<User>.Failure(Responses.InternalError);
			}
		}

		public async Task<Result<User>> GetUserByEmailAsync(string email)
		{
			try
			{
				var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

				if (user == null)
				{
					return Result<User>.Failure(Responses.UserNotFound);
				}

				return Result<User>.Success(user);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in GetUserByEmailAsync() in UserRepository: {ex.Message}");
				return Result<User>.Failure(Responses.InternalError);
			}
		}

		public async Task<Result<User>> GetUserByFirstNameAsync(string firstName)
		{
			try
			{
				var user = await _context.Users.FirstOrDefaultAsync(u => u.FirstName == firstName);

				if (user == null)
				{
					return Result<User>.Failure(Responses.UserNotFound);
				}

				return Result<User>.Success(user);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in GetUserByFirstNameAsync() in UserRepository: {ex.Message}");
				return Result<User>.Failure(Responses.InternalError);
			}
		}

		public async Task AddUserAsync(User user)
		{
			try
			{
				await _context.Users.AddAsync(user);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				Log.Error($"Error in AddUserAsync() in UserRepository: {ex.Message}");
			}
		}

		public async Task UpdateUserAsync(User user)
		{
			try
			{
				_context.Users.Update(user);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				Log.Error($"Error in UpdateUserAsync() in UserRepository: {ex.Message}");
			}
		}

		public async Task DeleteUserAsync(string id)
		{
			try
			{
				var user = await _context.Users.FindAsync(id);
				if (user != null)
				{
					_context.Users.Remove(user);
					await _context.SaveChangesAsync();
				}
			}
			catch (Exception ex)
			{
				Log.Error($"Error in DeleteUserAsync() in UserRepository: {ex.Message}");
			}
		}

		public async Task VerifyEmailAsync(User user)
		{
			user.EmailVerified = true;
			await UpdateUserAsync(user);
		}
	}
}