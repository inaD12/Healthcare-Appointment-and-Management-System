using Contracts.Abstractions.Messaging;
using Contracts.Results;
using Users.Application.Managers.Interfaces;

namespace Users.Application.Commands.Users.DeleteUser
{
	public sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
	{
		private readonly IRepositoryManager _repositotyManager;

		public DeleteUserCommandHandler(IRepositoryManager repositotyManager)
		{
			_repositotyManager = repositotyManager;
		}

		public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
		{
			var res = await _repositotyManager.User.GetUserByIdAsync(request.Id);

			if (res.IsFailure)
			{
				return Result.Failure(res.Response);
			}

			await _repositotyManager.User.DeleteUserAsync(request.Id);

			return Result.Success();
		}
	}
}
