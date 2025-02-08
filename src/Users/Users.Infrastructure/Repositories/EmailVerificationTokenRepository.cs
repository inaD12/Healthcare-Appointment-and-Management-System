﻿using Microsoft.EntityFrameworkCore;
using Shared.Domain.Results;
using Shared.Infrastructure.Repositories;
using Users.Domain.Abstractions.Repositories;
using Users.Domain.EmailVerification;
using Users.Domain.Responses;
using Users.Infrastructure.UsersDBContexts;

namespace Users.Infrastructure.Repositories;

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
