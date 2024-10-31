using Serilog;
using Users.Application.Auth.PasswordManager;
using Users.Application.Auth.TokenManager;
using Users.Application.Helpers.Interfaces;
using Users.Application.Managers.Interfaces;
using Users.Application.Services.Interfaces;
using Users.Domain.DTOs.Requests;
using Users.Domain.DTOs.Responses;
using Users.Domain.Entities;
using Users.Domain.Result;

namespace Users.Application.Services
{
    public class UserService : IUserService
	{
		private readonly IRepositoryManager _repositotyManager;
		private readonly IPasswordManager _passwordManager;
		private readonly ITokenManager _tokenManager;
		private readonly IEmailVerificationSender _emailVerificationSender;
		private readonly IFactoryManager _factoryManager;

		public UserService(IPasswordManager passwordManager, ITokenManager tokenManager, IRepositoryManager repositotyManager, IEmailVerificationSender emailVerificationSender = null, IFactoryManager factoryManager = null)
		{
			_passwordManager = passwordManager;
			_tokenManager = tokenManager;
			_repositotyManager = repositotyManager;
			_emailVerificationSender = emailVerificationSender;
			_factoryManager = factoryManager;
		}

		public async Task<Result<TokenDTO>> LoginAsync(LoginReqDTO loginDTO)
		{
			try
			{
				var res = await _repositotyManager.User.GetUserByEmailAsync(loginDTO.Email);

				if (res.IsFailure)
					return Result<TokenDTO>.Failure(res.Response);

				User user = res.Value;

				if (!_passwordManager.VerifyPassword(loginDTO.Password, user.PasswordHash, user.Salt))
					return Result<TokenDTO>.Failure(Response.IncorrectPassword);

				TokenDTO token = _tokenManager.CreateToken(user.Id);

				return Result<TokenDTO>.Success(token);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in LoginAsync() in UserService: {ex.Message} {ex.Source} {ex.InnerException}");
				return Result<TokenDTO>.Failure(Response.InternalError);
			}
		}

		public async Task<Result> RegisterAsync(RegisterReqDTO registerReqDTO)
		{
			try
			{
				var res = await _repositotyManager.User.GetUserByEmailAsync(registerReqDTO.Email);

				if (res.IsSuccess)
					return Result.Failure(Response.EmailTaken);

				User user = _factoryManager.UserFactory.CreateUser(
					registerReqDTO.Email,
					_passwordManager.HashPassword(registerReqDTO.Password, out string salt),
					salt,
					registerReqDTO.FirstName,
					registerReqDTO.LastName,
					registerReqDTO.DateOfBirth,
					registerReqDTO.PhoneNumber,
					registerReqDTO.Address
					);

				await _repositotyManager.User.AddUserAsync(user);

				Result emailSenderResult = await _emailVerificationSender.SendEmailAsync(user);

				if (emailSenderResult.IsFailure)
					return emailSenderResult;

				return Result.Success(Response.RegistrationSuccessful);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in RegisterAsync() in UserService: {ex.Message} {ex.Source} {ex.InnerException}");
				return Result.Failure(Response.InternalError);
			}
		}

		public async Task<Result> UpdateUserAsync(UpdateUserReqDTO updateDTO, string id)
		{
			try
			{
				var userResult = await _repositotyManager.User.GetUserByIdAsync(id);
				if (userResult.IsFailure)
					return Result.Failure(userResult.Response);

				User user = userResult.Value;

				if (!string.IsNullOrEmpty(updateDTO.NewEmail) && updateDTO.NewEmail != user.Email)
				{
					var emailCheckResult = await _repositotyManager.User.GetUserByEmailAsync(updateDTO.NewEmail);
					if (emailCheckResult.IsSuccess)
						return Result.Failure(Response.EmailTaken);

					user.Email = updateDTO.NewEmail;
				}

				user.FirstName = updateDTO.FirstName ?? user.FirstName;
				user.LastName = updateDTO.LastName ?? user.LastName;

				await _repositotyManager.User.UpdateUserAsync(user);

				return Result.Success(Response.UpdateSuccessful);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in UpdateUserAsync() in UserService: {ex.Message} {ex.Source} {ex.InnerException}");
				return Result.Failure(Response.InternalError);
			}
		}


		public async Task<Result> DeleteUserAsync(string id)
		{
			try
			{
				var res = await _repositotyManager.User.GetUserByIdAsync(id);

				if (res.IsFailure)
				{
					return Result.Failure(res.Response);
				}

				await _repositotyManager.User.DeleteUserAsync(id);

				return Result.Success();
			}
			catch (Exception ex)
			{
				Log.Error($"Error in DeleteUserAsync() in UserService: {ex.Message} {ex.Source} {ex.InnerException}");
				return Result.Failure(Response.InternalError);
			}
		}
	}

}
