using Shared.Application.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Users.Application.Features.Auth.Abstractions;
using Users.Application.Features.Auth.Models;
using Users.Application.Features.Users.Models;
using Users.Domain.Entities;
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
		var res = await _userRepository.GetByEmailAsync(request.Email);

		if (res.IsFailure)
			return Result<LoginUserCommandViewModel>.Failure(res.Response);

		User user = res.Value!;

		if (!_passwordManager.VerifyPassword(request.Password, user.PasswordHash, user.Salt))
			return Result<LoginUserCommandViewModel>.Failure(Responses.IncorrectPassword);

		TokenResult token = _tokenManager.CreateToken(user.Id);
		var loginUserCommandViewModel = _mapper.Map<LoginUserCommandViewModel>(token);
		return Result<LoginUserCommandViewModel>.Success(loginUserCommandViewModel);
	}
}
