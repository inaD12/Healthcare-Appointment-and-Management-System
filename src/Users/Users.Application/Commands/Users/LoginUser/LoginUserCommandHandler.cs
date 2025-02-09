using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Users.Application.Auth.PasswordManager;
using Users.Application.Auth.TokenManager;
using Users.Application.Managers.Interfaces;
using Users.Domain.DTOs.Responses;
using Users.Domain.Entities;
using Users.Domain.Responses;

namespace Users.Application.Commands.Users.LoginUser;

public sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand<TokenDTO>, TokenDTO>
{
	private readonly IRepositoryManager _repositotyManager;
	private readonly IPasswordManager _passwordManager;
	private readonly ITokenManager _tokenManager;

	public LoginUserCommandHandler(IRepositoryManager repositotyManager, IPasswordManager passwordManager, ITokenManager tokenManager)
	{
		_repositotyManager = repositotyManager;
		_passwordManager = passwordManager;
		_tokenManager = tokenManager;
	}

	public async Task<Result<TokenDTO>> Handle(LoginUserCommand<TokenDTO> request, CancellationToken cancellationToken)
	{
		var res = await _repositotyManager.User.GetByEmailAsync(request.Email);

		if (res.IsFailure)
			return Result<TokenDTO>.Failure(res.Response);

		User user = res.Value;

		if (!_passwordManager.VerifyPassword(request.Password, user.PasswordHash, user.Salt))
			return Result<TokenDTO>.Failure(Responses.IncorrectPassword);

		TokenDTO token = _tokenManager.CreateToken(user.Id);

		return Result<TokenDTO>.Success(token);
	}
}
