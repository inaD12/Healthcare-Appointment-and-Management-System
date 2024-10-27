using Users.Application.Factories;
using Users.Infrastructure.Repositories;

namespace Users.Infrastructure.DBContexts
{
	internal class DBContext : IDBContext
	{
		private readonly IRepositoryFactory _repositoryFactory;

		private IUserRepository _userRepository;
		private IEmailVerificationTokenRepository _emailVerificationTokenRepository;

		public DBContext(IRepositoryFactory repositoryFactory)
		{
			_repositoryFactory = repositoryFactory;
		}

		public IUserRepository User
			=> _userRepository ??= _repositoryFactory.CreateUserRepository();

		public IEmailVerificationTokenRepository EmailVerificationToken
			=> _emailVerificationTokenRepository ??= _repositoryFactory.CreateEmailTokenRepository();
	}
}
