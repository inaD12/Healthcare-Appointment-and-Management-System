using FluentEmail.Core;
using Serilog;
using Users.Application.Auth.PasswordManager;
using Users.Application.Auth.TokenManager;
using Users.Application.EmailVerification;
using Users.Application.Factories;
using Users.Application.Helpers;
using Users.Domain.DTOs.Requests;
using Users.Domain.DTOs.Responses;
using Users.Domain.EmailVerification;
using Users.Domain.Entities;
using Users.Domain.Result;
using Users.Infrastructure.DBContexts;

namespace Users.Application.Services
{
	public class UserService : IUserService
	{
		private readonly IDBContext _dbContext;
		private readonly IPasswordManager _passwordManager;
		private readonly ITokenManager _tokenManager;
		private readonly IEmailVerificationSender _emailVerificationSender;

		public UserService(IPasswordManager passwordManager, ITokenManager tokenManager, IDBContext dbContext, IEmailVerificationSender emailVerificationSender = null)
		{
			_passwordManager = passwordManager;
			_tokenManager = tokenManager;
			_dbContext = dbContext;
			_emailVerificationSender = emailVerificationSender;
		}

		public async Task<Result<TokenDTO>> LoginAsync(LoginReqDTO loginDTO)
		{
			var res = await _dbContext.User.GetUserByEmailAsync(loginDTO.Email);

			if (res.IsFailure)
				return Result<TokenDTO>.Failure(res.Response);

			User user = res.Value;

			if (!_passwordManager.VerifyPassword(loginDTO.Password, user.PasswordHash, user.Salt))
				return Result<TokenDTO>.Failure(Response.IncorrectPassword);

			TokenDTO token = _tokenManager.CreateToken(user.Id);

			return Result<TokenDTO>.Success(token);
		}

		public async Task<Result> RegisterAsync(RegisterReqDTO registerReqDTO)
		{
			try
			{
				var res = await _dbContext.User.GetUserByEmailAsync(registerReqDTO.Email);

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
					Role = "User",
					EmailVerified = false
				};

				await _dbContext.User.AddUserAsync(user);

				Result emailSenderResult = await _emailVerificationSender.SendEmailAsync(user);

				if (emailSenderResult.IsFailure)
					return emailSenderResult;

				return Result.Success(Response.RegistrationSuccessful);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in Register() in UserService: {ex.Message} {ex.Source} {ex.InnerException}");
				return Result.Failure(Response.InternalError);
			}
		}

		public async Task<Result> UpdateUserAsync(UpdateUserReqDTO updateDTO, string id)
		{
			var res = await _dbContext.User.GetUserByIdAsync(id);

			if (res.IsFailure)
				return Result.Failure(res.Response);

			User user = res.Value;

			bool newEmailIsCorrect = updateDTO.NewEmail != null
					&& user.Email != updateDTO.NewEmail
					&& (await _dbContext.User.GetUserByEmailAsync(updateDTO.NewEmail)).IsFailure;

			if (!newEmailIsCorrect)
			{
				return Result.Failure(Response.EmailTaken);
			}

			user.FirstName = updateDTO.FirstName ?? user.FirstName;
			user.LastName = updateDTO.LastName ?? user.LastName;
			user.Email = updateDTO.NewEmail ?? user.Email;

			await _dbContext.User.UpdateUserAsync(user);

			return Result.Success(Response.UpdateSuccessful);
		}

		public async Task<Result> DeleteUserAsync(string id)
		{
			var res = await _dbContext.User.GetUserByIdAsync(id);

			if (res.IsFailure)
			{
				return Result.Failure(res.Response);
			}

			await _dbContext.User.DeleteUserAsync(id);

			return Result.Success();
		}
	}

}
