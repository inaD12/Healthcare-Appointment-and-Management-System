using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Users.Application.Features.Auth.Abstractions;
using Users.Application.Features.Auth.Models;
using Users.Application.Features.Managers.Interfaces;
using Users.Domain.Entities;
using Users.Domain.Responses;

namespace Users.Application.Features.Users.LoginUser;

public sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand<TokenResult>, TokenResult>
{
	private readonly IRepositoryManager _repositotyManager;
	private readonly IPasswordManager _passwordManager;
	private readonly ITokenFactory _tokenManager;

	public LoginUserCommandHandler(IRepositoryManager repositotyManager, IPasswordManager passwordManager, ITokenFactory tokenManager)
	{
		_repositotyManager = repositotyManager;
		_passwordManager = passwordManager;
		_tokenManager = tokenManager;
	}

	public async Task<Result<TokenResult>> Handle(LoginUserCommand<TokenResult> request, CancellationToken cancellationToken)
	{
		var res = await _repositotyManager.User.GetByEmailAsync(request.Email);

		if (res.IsFailure)
			return Result<TokenResult>.Failure(res.Response);

		User user = res.Value!;

		if (!_passwordManager.VerifyPassword(request.Password, user.PasswordHash, user.Salt))
			return Result<TokenResult>.Failure(Responses.IncorrectPassword);

		TokenResult token = _tokenManager.CreateToken(user.Id);

		return Result<TokenResult>.Success(token);
	}
}
