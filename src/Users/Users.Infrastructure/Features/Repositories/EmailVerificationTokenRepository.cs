using Microsoft.EntityFrameworkCore;
using Shared.Domain.Results;
using Shared.Infrastructure.Repositories;
using Users.Domain.Entities;
using Users.Domain.Infrastructure.Abstractions.Repositories;
using Users.Domain.Responses;
using Users.Infrastructure.DBContexts;

namespace Users.Infrastructure.Features.Repositories;

internal class EmailVerificationTokenRepository : GenericRepository<EmailVerificationToken>, IEmailVerificationTokenRepository
{
	private readonly UsersDBContext _context;

	public EmailVerificationTokenRepository(UsersDBContext context) : base(context)
	{
		_context = context;
	}

	public override async Task<Result<EmailVerificationToken>> GetByIdAsync(string id)
	{
		EmailVerificationToken? token = await _context.EmailVerificationTokens
			.Include(x => x.User)
			.FirstOrDefaultAsync(x => x.Id == id);

		if (token is null)
			return Result<EmailVerificationToken>.Failure(Responses.TokenNotFound);

		return Result<EmailVerificationToken>.Success(token);
	}
}
