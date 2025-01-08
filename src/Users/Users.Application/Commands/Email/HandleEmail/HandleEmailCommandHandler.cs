using Contracts.Abstractions.Messaging;
using Contracts.Results;
using Users.Application.Managers.Interfaces;
using Users.Domain.Responses;

namespace Users.Application.Commands.Email.HandleEmail
{
	internal sealed class HandleEmailCommandHandler : ICommandHandler<HandleEmailCommand>
	{
		private readonly IRepositoryManager _repositoryManager;

		public HandleEmailCommandHandler(IRepositoryManager repositoryManager)
		{
			_repositoryManager = repositoryManager;
		}

		public async Task<Result> Handle(HandleEmailCommand request, CancellationToken cancellationToken)
		{
			var tokenResult = await _repositoryManager.EmailVerificationToken.GetTokenByIdAsync(request.tokenId);

			if (tokenResult.IsFailure ||
				tokenResult.Value.ExpiresOnUtc < DateTime.UtcNow ||
				tokenResult.Value.User.EmailVerified)
			{
				return Result.Failure(Responses.InvalidVerificationToken);
			}

			await _repositoryManager.User.VerifyEmailAsync(tokenResult.Value.User);

			return Result.Success(Response.Ok);
		}
	}
}
