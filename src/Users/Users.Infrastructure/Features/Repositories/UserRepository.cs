﻿using Microsoft.EntityFrameworkCore;
using Shared.Domain.Enums;
using Shared.Domain.Results;
using Shared.Infrastructure.Repositories;
using Users.Domain.Abstractions.Repositories;
using Users.Domain.Entities;
using Users.Domain.Responses;
using Users.Infrastructure.DBContexts;

namespace Users.Infrastructure.Features.Repositories;

internal class UserRepository : GenericRepository<User>, IUserRepository
{
	private readonly UsersDBContext _context;

	public UserRepository(UsersDBContext context) : base(context)
	{
		_context = context;
	}

	public async Task<Result<IEnumerable<User>>> GetAllDoctorsAsync()
	{
		var users = await _context.Users.Where(e => e.Role == Roles.Doctor).ToListAsync();

		if (!users.Any())
			return Result<IEnumerable<User>>.Failure(Responses.NoUsersFound);

		return Result<IEnumerable<User>>.Success(users);
	}

	public async Task<Result<User>> GetByEmailAsync(string email)
	{
		var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

		if (user == null)
			return Result<User>.Failure(Responses.UserNotFound);

		return Result<User>.Success(user);
	}

	public async Task<Result<User>> GetByFirstNameAsync(string firstName)
	{
		var user = await _context.Users.FirstOrDefaultAsync(u => u.FirstName == firstName);

		if (user == null)
			return Result<User>.Failure(Responses.UserNotFound);

		return Result<User>.Success(user);
	}

	public async Task<Result> DeleteByIdAsync(string id)
	{
		var res = await GetByIdAsync(id);

		if (res.IsSuccess)
		{
			DeleteAsync(res.Value!);
			return Result.Success();
		}

		return Result.Failure(res.Response);
	}

	public override async Task AddAsync(User user)
	{
		await _context.Users.AddAsync(user);
	}

	public void VerifyEmailAsync(User user)
	{
		user.EmailVerified = true;
		base.UpdateAsync(user);
	}
}