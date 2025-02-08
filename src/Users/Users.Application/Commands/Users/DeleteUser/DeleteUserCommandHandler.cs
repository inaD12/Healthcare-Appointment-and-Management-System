using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Users.Application.Managers.Interfaces;

namespace Users.Application.Commands.Users.DeleteUser;

public sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
{
	private readonly IRepositoryManager _repositotyManager;

	public DeleteUserCommandHandler(IRepositoryManager repositotyManager)
	{
		_repositotyManager = repositotyManager;
	}

	public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
	{
		var res = await _repositotyManager.User.GetByIdAsync(request.Id);

		if (res.IsFailure)
		{
			return Result.Failure(res.Response);
		}

		var result = await _repositotyManager.User.DeleteByIdAsync(request.Id);

		return result;
	}
}
