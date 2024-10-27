using Users.Infrastructure.Repositories;

namespace Users.Application.Factories
{
	public interface IRepositoryFactory
	{
		IEmailVerificationTokenRepository CreateEmailTokenRepository();
		IUserRepository CreateUserRepository();
	}
}