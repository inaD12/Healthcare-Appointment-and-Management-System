using Shared.Domain.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Shared.Infrastructure.Clock;
using Users.Domain.Infrastructure.Abstractions.Repositories;
using Users.Domain.Responses;

namespace Users.Application.Features.Email.Commands.HandleEmail;

public sealed class HandleEmailCommandHandler : ICommandHandler<HandleEmailCommand>
{
	private readonly IEmailVerificationTokenRepository _emailVerificationTokenRepository;
	private readonly IUserRepository _userRepository;
	private readonly IUnitOfWork _unitOfWork;
	private readonly IDateTimeProvider _dateTimeProvider;

	public HandleEmailCommandHandler(IUnitOfWork unitOfWork, IEmailVerificationTokenRepository emailVerificationTokenRepository, IUserRepository userRepository, IDateTimeProvider dateTimeProvider)
	{
		_unitOfWork = unitOfWork;
		_emailVerificationTokenRepository = emailVerificationTokenRepository;
		_userRepository = userRepository;
		_dateTimeProvider = dateTimeProvider;
	}

	public async Task<Result> Handle(HandleEmailCommand request, CancellationToken cancellationToken)
	{
		var token = await _emailVerificationTokenRepository.GetByIdAsync(request.tokenId);

		if (token == null)
			return Result.Failure(ResponseList.InvalidVerificationToken);
		if(token.ExpiresOnUtc < _dateTimeProvider.UtcNow)
			return Result.Failure(ResponseList.ExpiredVerificationToken);
		if (token.User.EmailVerified)
			return Result.Failure(ResponseList.EmailAlreadyVerified);

		token.User.VerifyEmail();
		 _userRepository.Update(token.User);

		await _unitOfWork.SaveChangesAsync();
		return Result.Success();
	}
}
