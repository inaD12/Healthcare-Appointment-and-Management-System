using Users.Infrastructure.Repositories;

namespace Users.Infrastructure.DBContexts
{
	public interface IDBContext
	{
		IEmailVerificationTokenRepository EmailVerificationToken { get; }
		IUserRepository User { get; }
	}
}