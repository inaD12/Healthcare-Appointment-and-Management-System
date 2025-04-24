using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Users.Domain.Infrastructure.Abstractions.Repositories;
using Users.Domain.Responses;

namespace Users.Application.Features.Users.Commands.DeleteUser;

public sealed class DeleteUserCommandHandler : ICommandHandler<DeleteUserCommand>
{
	private readonly IUserRepository _userRepository;
	private readonly IUnitOfWork _unitOfWork;

	public DeleteUserCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
	{
		_unitOfWork = unitOfWork;
		_userRepository = userRepository;
	}

	public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByIdAsync(request.Id);
		if (user == null)
			return Result.Failure(ResponseList.UserNotFound);

		_userRepository.Delete(user);
		await _unitOfWork.SaveChangesAsync();
		return Result.Success();
	}
}
