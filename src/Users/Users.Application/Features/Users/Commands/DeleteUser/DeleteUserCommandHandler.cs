using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Users.Domain.Abstractions.Repositories;
using Users.Domain.Utilities;

namespace Users.Application.Features.Users.Commands.DeleteUser;

public sealed class DeleteUserCommandHandler(IUnitOfWork unitOfWork, IUserRepository userRepository)
	: ICommandHandler<DeleteUserCommand>
{
	public async Task<Result> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
	{
		var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);
		if (user == null)
			return Result.Failure(ResponseList.UserNotFound);

		userRepository.Delete(user);
		await unitOfWork.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}
}
