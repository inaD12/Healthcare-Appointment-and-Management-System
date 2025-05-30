using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Shared.Domain.Models;
using Shared.Domain.Results;
using Shared.Infrastructure.Extensions;
using Shared.Infrastructure.Repositories;
using Users.Domain.Entities;
using Users.Domain.Infrastructure.Abstractions.Repositories;
using Users.Domain.Infrastructure.Models;
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

	public async Task<PagedList<User>?> GetAllAsync(UserPagedListQuery query, CancellationToken cancellationToken)
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

		if (entitiesQuery.IsNullOrEmpty())
			return null;

		var users = await PagedList<User>.CreateAsync(entitiesQuery, query.Page, query.PageSize, cancellationToken);
		return users;
	}


	public async Task<User?> GetByEmailAsync(string email)
	{
		var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

		return user;
	}
}