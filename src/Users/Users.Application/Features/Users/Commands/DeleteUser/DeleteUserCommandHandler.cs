using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Shared.Infrastructure.Abstractions;
using Users.Application.Features.Managers.Interfaces;

namespace Users.Application.Features.Users.Commands.DeleteUser;

public sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
{
	private readonly IRepositoryManager _repositotyManager;
	private readonly IUnitOfWork _unitOfWork;

	public DeleteUserCommandHandler(IRepositoryManager repositotyManager, IUnitOfWork unitOfWork)
	{
		_repositotyManager = repositotyManager;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
	{
		var res = await _repositotyManager.User.GetByIdAsync(request.Id);

		if (res.IsFailure)
			return Result.Failure(res.Response);

		var result = await _repositotyManager.User.DeleteByIdAsync(request.Id);

		await _unitOfWork.SaveChangesAsync();
		return result;
	}
}
