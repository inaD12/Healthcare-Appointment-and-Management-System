using System.Collections.Generic;
using Users.Domain;
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
			IEnumerable<User> users = _context.Users.ToList();

			if (users.Any())
			{
				return Result<IEnumerable<User>>.Failure(ResponseMessages.NoUsersFound);
			}

			return Result<IEnumerable<User>>.Success(users); 
		}

		public Result<User> GetUserById(string id)
		{
			User user = _context.Users.Find(id);

			if (user is null)
			{
				return Result<User>.Failure(ResponseMessages.UserNotFound);
			}

			return Result<User>.Success(user);
		}

		public Result<User> GetUserByEmail(string email)
		{
			User user = _context.Users.FirstOrDefault(u => u.Email == email);

			if (user is null)
			{
				return Result<User>.Failure(ResponseMessages.UserNotFound);
			}

			return Result<User>.Success(user);
		}

		public Result<User> GetUserByFirstName(string FirstName)
		{
			User user = _context.Users.FirstOrDefault(u => u.FirstName == FirstName);

			if (user is null)
			{
				return Result<User>.Failure(ResponseMessages.UserNotFound);
			}

			return Result<User>.Success(user);
		}

		public void AddUser(User user)
		{
			 _context.Users.Add(user);
			_context.SaveChanges();
		}

		public void UpdateUser(User user)
		{
			_context.Users.Update(user);
			 _context.SaveChanges();
		}

		public void DeleteUser(int id)
		{
			var user =  _context.Users.Find(id);
			if (user != null)
			{
				_context.Users.Remove(user);
				_context.SaveChanges();
			}
		}
	}
}
