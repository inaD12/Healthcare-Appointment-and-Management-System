using Users.Infrastructure.Repositories;
using Users.Infrastructure.UsersDBContexts;

namespace Users.Application.Factories
{
	public class RepositoryFactory : IRepositoryFactory
	{
		private readonly UsersDBContext _dbContext;

		public RepositoryFactory(UsersDBContext dbContext)
		{
			_dbContext = dbContext;
		}

		public IEmailVerificationTokenRepository CreateEmailTokenRepository()
		{
			return new EmailVerificationTokenRepository(_dbContext);
		}

		public IUserRepository CreateUserRepository()
		{
			return new UserRepository(_dbContext);
		}
	}
}
