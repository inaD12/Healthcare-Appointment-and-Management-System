using Shared.Application.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Users.Application.Features.Users.Models;
using Users.Domain.Auth.Abstractions;
using Users.Domain.Auth.Models;
using Users.Domain.Infrastructure.Abstractions.Repositories;
using Users.Domain.Responses;

namespace Users.Application.Features.Users.LoginUser;

public sealed class LoginUserCommandHandler : ICommandHandler<LoginUserCommand, LoginUserCommandViewModel>
{
	private readonly IUserRepository _userRepository;
	private readonly IPasswordManager _passwordManager;
	private readonly ITokenFactory _tokenManager;
	private readonly IHAMSMapper _mapper;

	public LoginUserCommandHandler(IPasswordManager passwordManager, ITokenFactory tokenManager, IHAMSMapper mapper, IUserRepository userRepository)
	{
		_passwordManager = passwordManager;
		_tokenManager = tokenManager;
		_mapper = mapper;
		_userRepository = userRepository;
	}

	public async Task<Result<LoginUserCommandViewModel>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByEmailAsync(request.Email);

		if (user == null)
			return Result<LoginUserCommandViewModel>.Failure(ResponseList.UserNotFound);

		if (!_passwordManager.VerifyPassword(request.Password, user.PasswordHash, user.Salt))
			return Result<LoginUserCommandViewModel>.Failure(ResponseList.IncorrectPassword);

		TokenResult token = _tokenManager.CreateToken(user.Id, user.Role);
		var loginUserCommandViewModel = _mapper.Map<LoginUserCommandViewModel>(token);
		return Result<LoginUserCommandViewModel>.Success(loginUserCommandViewModel);
	}
}
