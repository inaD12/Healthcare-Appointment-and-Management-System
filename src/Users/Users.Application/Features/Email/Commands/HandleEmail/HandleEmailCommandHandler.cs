using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Users.Domain.Infrastructure.Abstractions.Repositories;
using Users.Domain.Responses;

namespace Users.Application.Features.Email.Commands.HandleEmail;

public sealed class HandleEmailCommandHandler : ICommandHandler<HandleEmailCommand>
{
	private readonly IEmailVerificationTokenRepository _emailVerificationTokenRepository;
	private readonly IUserRepository _userRepository;
	private readonly IUnitOfWork _unitOfWork;

	public HandleEmailCommandHandler(IUnitOfWork unitOfWork, IEmailVerificationTokenRepository emailVerificationTokenRepository, IUserRepository userRepository)
	{
		_unitOfWork = unitOfWork;
		_emailVerificationTokenRepository = emailVerificationTokenRepository;
		_userRepository = userRepository;
	}

	public async Task<Result> Handle(HandleEmailCommand request, CancellationToken cancellationToken)
	{
		var tokenResult = await _emailVerificationTokenRepository.GetByIdAsync(request.tokenId);

		var token = tokenResult.Value!;

		if (tokenResult.IsFailure ||
			token.ExpiresOnUtc < DateTime.UtcNow ||
			token.User.EmailVerified)
		{
			return Result.Failure(ResponseList.InvalidVerificationToken);
		}

		token.User.VerifyEmail();
		 _userRepository.Update(token.User);

		await _unitOfWork.SaveChangesAsync();
		return Result.Success();
	}
}
