using Microsoft.EntityFrameworkCore;
using Shared.Domain.Models;
using Shared.Domain.Results;
using Shared.Infrastructure.Extensions;
using Shared.Infrastructure.Repositories;
using Users.Domain.Abstractions.Repositories;
using Users.Domain.Entities;
using Users.Domain.Models;
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

	public async Task<Result<PagedList<User>>> GetAllAsync(UserPagedListQuery query, CancellationToken cancellationToken)
	{
		var entitiesQuery = _context.Users
			.Where(u =>
				(string.IsNullOrEmpty(query.FirstName) || u.FirstName.StartsWith(query.FirstName)) &&
				(string.IsNullOrEmpty(query.LastName) || u.LastName.StartsWith(query.LastName)) &&
				(string.IsNullOrEmpty(query.Email) || u.Email.StartsWith(query.Email)) &&
				(!query.Role.HasValue || u.Role == query.Role!.Value) &&
				(string.IsNullOrEmpty(query.PhoneNumber) || u.PhoneNumber!.StartsWith(query.PhoneNumber)) &&
				(string.IsNullOrEmpty(query.Address) || u.Address!.StartsWith(query.Address)) &&
				(!query.EmailVerified.HasValue || u.EmailVerified == query.EmailVerified!.Value)
			).ApplySorting(query.SortPropertyName, query.SortOrder);

		if (entitiesQuery == null)
			return Result<PagedList<User>>.Failure(Responses.NoUsersFound);

		var users = await PagedList<User>.CreateAsync(entitiesQuery, query.Page, query.PageSize, cancellationToken);
		return Result<PagedList<User>>.Success(users);
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