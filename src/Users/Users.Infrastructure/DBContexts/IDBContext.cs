using Users.Infrastructure.Repositories;

namespace Users.Infrastructure.DBContexts
{
	internal interface IDBContext
	{
		IEmailVerificationTokenRepository EmailVerificationToken { get; }
		IUserRepository User { get; }
	}
}