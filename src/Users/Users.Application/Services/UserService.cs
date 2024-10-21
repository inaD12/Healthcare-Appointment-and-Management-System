using Users.Application.Auth.PasswordManager;
using Users.Application.Auth.TokenManager;
using Users.Domain;
using Users.Domain.DTOs.Requests;
using Users.Domain.DTOs.Responses;
using Users.Domain.Entities;
using Users.Domain.Result;
using Users.Infrastructure.Repositories;

namespace Users.Application.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IPasswordManager _passwordManager;
		private readonly ITokenManager _tokenManager;
		public UserService(IUserRepository userRepository, IPasswordManager passwordManager, ITokenManager tokenManager)
		{
			_userRepository = userRepository;
			_passwordManager = passwordManager;
			_tokenManager = tokenManager;
		}

		public Result<TokenDTO> Login(LoginReqDTO loginDTO)
		{
			var res = _userRepository.GetUserByEmail(loginDTO.Email);

			if (res.IsFailure)
				return Result<TokenDTO>.Failure(res.Response);

			User user = res.Value;

			if (!_passwordManager.VerifyPassword(loginDTO.Password, user.PasswordHash, user.Salt))
				return Result<TokenDTO>.Failure(Response.IncorrectPassword);

			TokenDTO Token = _tokenManager.CreateToken(user.Id);

			return Result<TokenDTO>.Success(Token);
		}

		public Result Register(RegisterReqDTO registerReqDTO)
		{
			var res = _userRepository.GetUserByEmail(registerReqDTO.Email);

			if (res.IsSuccess)
				return Result.Failure(Response.EmailTaken);

			User user = new()
			{
				Id = Guid.NewGuid().ToString(),
				Email = registerReqDTO.Email,
				FirstName = registerReqDTO.FirstName,
				LastName = registerReqDTO.LastName,
				PasswordHash = _passwordManager.HashPassword(registerReqDTO.Password, out string salt),
				Salt = salt,
				DateOfBirth = registerReqDTO.DateOfBirth,
				PhoneNumber = registerReqDTO.PhoneNumber,
				Address = registerReqDTO.Address,
				Role = "User"
			};

			_userRepository.AddUser(user);
			//if (!_emailConfirmation.SendEmail(user.Email, user.Id))
			//{
			//	return ResponseMessages.UserBadEmailSyntax;
			//}

			return Result.Success(Response.RegistrationSuccessful);
		}

		public Result UpdateUser(UpdateUserReqDTO updateDTO, string id)
		{
			var res = _userRepository.GetUserById(id);

			if (res.IsFailure)
				return Result.Failure(res.Response);

			User user = res.Value;

			bool newEmailExists = updateDTO.NewEmail != null
					&& user.Email != updateDTO.NewEmail
					&& _userRepository.GetUserByEmail(updateDTO.NewEmail).IsFailure;

			if (newEmailExists)
			{
				return Result.Failure(Response.EmailTaken);
			}

			user.FirstName = updateDTO.FirstName ?? user.FirstName;
			user.LastName = updateDTO.LastName ?? user.LastName;
			user.Email = updateDTO.NewEmail ?? user.Email;

			_userRepository.UpdateUser(user);

			return Result.Success(Response.UpdateSuccessful);
		}

		public Result DeleteUser(DeleteReqDTO deleteReqDTO)
		{
			var res = _userRepository.GetUserById(deleteReqDTO.Id);

			User user = res.Value;

			if (res.IsFailure)
			{
				return Result.Failure(res.Response);
			}

			return Result.Success();
		}
	}
}
