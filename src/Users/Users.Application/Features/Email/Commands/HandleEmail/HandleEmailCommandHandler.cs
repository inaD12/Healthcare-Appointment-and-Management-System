using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Shared.Infrastructure.Abstractions;
using Users.Application.Features.Managers.Interfaces;
using Users.Domain.Responses;

namespace Users.Application.Features.Email.Commands.HandleEmail;

public sealed class HandleEmailCommandHandler : ICommandHandler<HandleEmailCommand>
{
	private readonly IRepositoryManager _repositoryManager;
	private readonly IUnitOfWork _unitOfWork;

	public HandleEmailCommandHandler(IRepositoryManager repositoryManager, IUnitOfWork unitOfWork)
	{
		_repositoryManager = repositoryManager;
		_unitOfWork = unitOfWork;
	}

	public async Task<Result> Handle(HandleEmailCommand request, CancellationToken cancellationToken)
	{
		var tokenResult = await _repositoryManager.EmailVerificationToken.GetByIdAsync(request.tokenId);

		var token = tokenResult.Value!;

		if (tokenResult.IsFailure ||
			token.ExpiresOnUtc < DateTime.UtcNow ||
			token.User.EmailVerified)
		{
			return Result.Failure(Responses.InvalidVerificationToken);
		}

		await _repositoryManager.User.VerifyEmailAsync(token.User);

		await _unitOfWork.SaveChangesAsync();
		return Result.Success();
	}
}
