using FluentEmail.Core;
using Serilog;
using Users.Application.Auth.PasswordManager;
using Users.Application.Auth.TokenManager;
using Users.Application.EmailVerification;
using Users.Application.Factories;
using Users.Domain.DTOs.Requests;
using Users.Domain.DTOs.Responses;
using Users.Domain.EmailVerification;
using Users.Domain.Entities;
using Users.Domain.Result;
using Users.Infrastructure.Repositories;

namespace Users.Application.Services
{
	public class UserService : IUserService
	{
		private readonly IUserRepository _userRepository;
		private readonly IEmailVerificationTokenRepository _emailVerificationTokenRepository;
		private readonly IPasswordManager _passwordManager;
		private readonly ITokenManager _tokenManager;
		private readonly IFluentEmail _fluentEmail;
		private readonly IEmailVerificationLinkFactory _emailVerificationLinkFactory;
		private readonly IEmailVerificationTokenFactory _emailVerificationTokenFactory;
		public UserService(IUserRepository userRepository, IPasswordManager passwordManager, ITokenManager tokenManager, IFluentEmail fluentEmail, IEmailVerificationTokenRepository emailVerificationTokenRepository, IEmailVerificationLinkFactory emailVerificationLinkFactory)
		{
			_userRepository = userRepository;
			_passwordManager = passwordManager;
			_tokenManager = tokenManager;
			_fluentEmail = fluentEmail;
			_emailVerificationTokenRepository = emailVerificationTokenRepository;
			_emailVerificationLinkFactory = emailVerificationLinkFactory;
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
			try
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
					Role = "User",
					EmailVerified = false
				};

				_userRepository.AddUser(user);

				DateTime utcNow = DateTime.UtcNow;

				EmailVerificationToken emailVerificationToken = _emailVerificationTokenFactory.CreateToken(
					Guid.NewGuid().ToString(),
					user.Id,
					utcNow,
					utcNow.AddDays(1));

				_emailVerificationTokenRepository.AddToken(emailVerificationToken);

				string verificationLink = _emailVerificationLinkFactory.Create(emailVerificationToken);

				_fluentEmail
				   .To(user.Email)
				   .Subject("Email verifivation for HAMS")
				   .Body($"To verify your email <a href='{verificationLink}'>click here</a>", isHtml:true)
				   .Send();

				return Result.Success(Response.RegistrationSuccessful);
			}
			catch (Exception ex)
			{
				Log.Error($"Error in Register() in UserService: {ex.Message} {ex.Source} {ex.InnerException}");
				return Result.Failure(Response.InternalError);
			}
		}

		public Result UpdateUser(UpdateUserReqDTO updateDTO, string id)
		{
			var res = _userRepository.GetUserById(id);

			if (res.IsFailure)
				return Result.Failure(res.Response);

			User user = res.Value;

			bool newEmailIsCorrect = updateDTO.NewEmail != null
					&& user.Email != updateDTO.NewEmail
					&& _userRepository.GetUserByEmail(updateDTO.NewEmail).IsFailure;

			if (!newEmailIsCorrect)
			{
				return Result.Failure(Response.EmailTaken);
			}

			user.FirstName = updateDTO.FirstName ?? user.FirstName;
			user.LastName = updateDTO.LastName ?? user.LastName;
			user.Email = updateDTO.NewEmail ?? user.Email;

			_userRepository.UpdateUser(user);

			return Result.Success(Response.UpdateSuccessful);
		}

		public Result DeleteUser(string id)
		{
			var res = _userRepository.GetUserById(id);

			if (res.IsFailure)
			{
				return Result.Failure(res.Response);
			}

			User user = res.Value;

			_userRepository.DeleteUser(id);

			return Result.Success();
		}
	}
}
