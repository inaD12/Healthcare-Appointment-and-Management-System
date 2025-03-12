using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Shared.Infrastructure.Abstractions;
using Users.Domain.Abstractions.Repositories;

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
		var res = await _userRepository.GetByIdAsync(request.Id);

		if (res.IsFailure)
			return Result.Failure(res.Response);

		var result = await _userRepository.DeleteByIdAsync(request.Id);

		await _unitOfWork.SaveChangesAsync();
		return result;
	}
}
