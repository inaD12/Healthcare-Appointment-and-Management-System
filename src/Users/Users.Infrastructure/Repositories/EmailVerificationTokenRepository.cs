using Contracts.Results;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Users.Domain.EmailVerification;
using Users.Domain.Responses;
using Users.Infrastructure.Repositories.Interfaces;
using Users.Infrastructure.UsersDBContexts;

namespace Users.Infrastructure.Repositories
{
    public class EmailVerificationTokenRepository : IEmailVerificationTokenRepository
	{
		private readonly UsersDBContext _context;

		public EmailVerificationTokenRepository(UsersDBContext context)
		{
			_context = context;
		}

		public async Task AddTokenAsync(EmailVerificationToken token)
		{
			try
			{
				await _context.EmailVerificationTokens.AddAsync(token);
				await _context.SaveChangesAsync();
			}
			catch (Exception ex)
			{
				Log.Error($"Error in AddToken() in EmailVerificationTokenRepository: {ex.Message}");
			}
		}

		public async Task<Result<EmailVerificationToken>> GetTokenByIdAsync(string id)
		{
			try
			{
				EmailVerificationToken? token =await _context.EmailVerificationTokens
					.Include(x => x.User)
					.FirstOrDefaultAsync(x => x.Id == id);

				if (token is null)
				{
					return Result<EmailVerificationToken>.Failure(Responses.TokenNotFound);
				}

				return Result<EmailVerificationToken>.Success(token);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in GetTokenById() in EmailVerificationTokenRepository: {ex.Message}");
				return Result<EmailVerificationToken>.Failure(Responses.InternalError);
			}
		}
	}
}
