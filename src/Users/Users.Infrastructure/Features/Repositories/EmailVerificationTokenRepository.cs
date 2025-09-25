using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Repositories;
using Users.Domain.Entities;
using Users.Domain.Infrastructure.Abstractions.Repositories;
using Users.Infrastructure.Features.DBContexts;

namespace Users.Infrastructure.Features.Repositories;

internal class EmailVerificationTokenRepository : GenericRepository<EmailVerificationToken>, IEmailVerificationTokenRepository
{
	private readonly UsersDbContext _context;

	public EmailVerificationTokenRepository(UsersDbContext context) : base(context)
	{
		_context = context;
	}

	public override async Task<EmailVerificationToken?> GetByIdAsync(string id)
	{
		EmailVerificationToken? token = await _context.EmailVerificationTokens
			.Include(x => x.User)
			.FirstOrDefaultAsync(x => x.Id == id);

		return token;
	}
}
