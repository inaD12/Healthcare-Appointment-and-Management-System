using Microsoft.EntityFrameworkCore;
using Shared.Domain.Models;
using Shared.Infrastructure.Extensions;
using Shared.Infrastructure.Repositories;
using Users.Domain.Entities;
using Users.Domain.Infrastructure.Abstractions.Repositories;
using Users.Domain.Infrastructure.Models;
using Users.Infrastructure.Features.DBContexts;

namespace Users.Infrastructure.Features.Repositories;

internal class UserRepository : GenericRepository<User>, IUserRepository
{
	private readonly UsersDbContext _context;

	public UserRepository(UsersDbContext context) : base(context)
	{
		_context = context;
	}

	public async Task<PagedList<User>?> GetAllAsync(UserPagedListQuery query, CancellationToken cancellationToken)
	{
		var entitiesQuery = _context.Users.AsQueryable();

		if (!string.IsNullOrWhiteSpace(query.FirstName))
			entitiesQuery = entitiesQuery.Where(u => EF.Functions.ILike(u.FirstName, $"{query.FirstName}%"));

		if (!string.IsNullOrWhiteSpace(query.LastName))
			entitiesQuery = entitiesQuery.Where(u => EF.Functions.ILike(u.LastName, $"{query.LastName}%"));

		if (!string.IsNullOrWhiteSpace(query.Email))
			entitiesQuery = entitiesQuery.Where(u => EF.Functions.ILike(u.Email, $"{query.Email}%"));

		if (!string.IsNullOrWhiteSpace(query.PhoneNumber))
			entitiesQuery = entitiesQuery.Where(u => EF.Functions.ILike(u.PhoneNumber!, $"{query.PhoneNumber}%"));

		if (!string.IsNullOrWhiteSpace(query.Address))
			entitiesQuery = entitiesQuery.Where(u => EF.Functions.ILike(u.Address!, $"{query.Address}%"));

		if (query.Role != null)
			entitiesQuery = entitiesQuery.Where(u => u.Roles.Contains(query.Role));

		if (query.EmailVerified.HasValue)
			entitiesQuery = entitiesQuery.Where(u => u.EmailVerified == query.EmailVerified.Value);

		entitiesQuery = entitiesQuery.ApplySorting(query.SortPropertyName, query.SortOrder);

		if (!await entitiesQuery.AnyAsync(cancellationToken))
			return null;

		var users = await PagedList<User>.CreateAsync(entitiesQuery, query.Page, query.PageSize, cancellationToken);
		return users;
	}

	public async Task<User?> GetByEmailAsync(string email)
	{
		var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

		return user;
	}

	public override Task AddAsync(User entity)
	{
		foreach (var role in entity.Roles)
		{
			_context.Attach(role);
		}

		return base.AddAsync(entity);
	}
}