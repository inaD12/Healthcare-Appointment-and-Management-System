using Shared.Application.Abstractions;
using Shared.Application.Abstractions.Messaging;
using Shared.Domain.Results;
using Users.Application.Features.Users.Abstractions;
using Users.Domain.Utilities;

namespace Users.Application.Features.Email.Commands.HandleEmail;

public sealed class HandleEmailCommandHandler(
	IUnitOfWork unitOfWork,
	IEmailVerificationTokenRepository emailVerificationTokenRepository,
	IUserRepository userRepository,
	IDateTimeProvider dateTimeProvider)
	: ICommandHandler<HandleEmailCommand>
{
	public async Task<Result> Handle(HandleEmailCommand request, CancellationToken cancellationToken)
	{
		var token = await emailVerificationTokenRepository.GetByIdAsync(request.TokenId, cancellationToken);

		if (token == null)
			return Result.Failure(ResponseList.InvalidVerificationToken);
		if(token.ExpiresOnUtc < dateTimeProvider.UtcNow)
			return Result.Failure(ResponseList.ExpiredVerificationToken);
		if (token.User!.EmailVerified)
			return Result.Failure(ResponseList.EmailAlreadyVerified);

		token.User.VerifyEmail();
		 userRepository.Update(token.User);

		await unitOfWork.SaveChangesAsync(cancellationToken);
		return Result.Success();
	}
}
