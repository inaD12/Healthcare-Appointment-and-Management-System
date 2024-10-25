using Serilog;
using Users.Domain.Entities;
using Users.Domain.Result;
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

		public Result<IEnumerable<User>> GetAllUsers()
		{
			try
			{
				IEnumerable<User> users = _context.Users.ToList();

				if (users.Any())
				{
					return Result<IEnumerable<User>>.Failure(Response.NoUsersFound);
				}

				return Result<IEnumerable<User>>.Success(users);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in GetAllUsers() in UserRepository: {ex.Message}");
				return Result<IEnumerable<User>>.Failure(Response.InternalError);
			}
		}

		public Result<User> GetUserById(string id)
		{
			try
			{
				User? user = _context.Users.Find(id);

				if (user is null)
				{
					return Result<User>.Failure(Response.UserNotFound);
				}

				return Result<User>.Success(user);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in GetUserById() in UserRepository: {ex.Message}");
				return Result<User>.Failure(Response.InternalError);
			}
		}

		public Result<User> GetUserByEmail(string email)
		{
			try
			{
				User? user = _context.Users.FirstOrDefault(u => u.Email == email);

				if (user is null)
				{
					return Result<User>.Failure(Response.UserNotFound);
				}

				return Result<User>.Success(user);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in GetUserByEmail() in UserRepository: {ex.Message}");
				return Result<User>.Failure(Response.InternalError);
			}
		}

		public Result<User> GetUserByFirstName(string FirstName)
		{
			try
			{
				User? user = _context.Users.FirstOrDefault(u => u.FirstName == FirstName);

				if (user is null)
				{
					return Result<User>.Failure(Response.UserNotFound);
				}

				return Result<User>.Success(user);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in GetUserByEmail() in UserRepository: {ex.Message}");
				return Result<User>.Failure(Response.InternalError);
			}
		}

		public void AddUser(User user)
		{
			try
			{
				_context.Users.Add(user);
				_context.SaveChanges();
			}
			catch (Exception ex)
			{
				Log.Error($"Error in AddUser() in UserRepository: {ex.Message}");
			}
		}

		public void UpdateUser(User user)
		{
			try
			{
				_context.Users.Update(user);
				_context.SaveChanges();
			}
			catch (Exception ex)
			{
				Log.Error($"Error in UpdateUser() in UserRepository: {ex.Message}");
			}
		}

		public void DeleteUser(string id)
		{
			try
			{
				var user = _context.Users.Find(id);
				if (user != null)
				{
					_context.Users.Remove(user);
					_context.SaveChanges();
				}
			}
			catch (Exception ex)
			{
				Log.Error($"Error in DeleteUser() in UserRepository: {ex.Message}");
			}
		}

		public void VerifyEmail(User user)
		{
			user.EmailVerified = true;
			UpdateUser(user);
		}
	}
}
