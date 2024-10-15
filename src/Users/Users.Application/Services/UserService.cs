using Users.Application.Auth.PasswordManager;
using Users.Application.Auth.TokenManager;
using Users.Domain;
using Users.Domain.DTOs.Requests;
using Users.Domain.Entities;
using Users.Domain.Result;
using Users.Infrastructure.Repositories;

namespace Users.Application.Services
{
	public class UserService
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

        public Result<string> Login(LoginReqDTO loginDTO)
        {
            var res = _userRepository.GetUserByEmail(loginDTO.Email);

            if (res.IsFailure)
                return Result<string>.Failure(res.Description);

            User user = res.Value;

            if (!_passwordManager.VerifyPassword(loginDTO.Password, user.PasswordHash, user.Salt))
                return Result<string>.Failure(ResponseMessages.IncorrectPass);

            string Token = _tokenManager.CreateToken(user.FirstName, user.Role);

            return Result<string>.Success(Token);
        }

        public Result Register(RegisterReqDTO registerReqDTO)
        {
            var res = _userRepository.GetUserByEmail(registerReqDTO.Email);

            if (res.IsSuccess)
                return Result.Failure(ResponseMessages.EmailTaken);

            User user = new()
            {
                Email = registerReqDTO.Email,
                FirstName = registerReqDTO.FirstName,
                LastName = registerReqDTO.LastName,
                PasswordHash = _passwordManager.HashPassword(registerReqDTO.Password, out string salt),
                Salt = salt,
                DateOfBirth = registerReqDTO.DateOfBirth,
                PhoneNumber = registerReqDTO.PhoneNumber,
                Address = registerReqDTO.Address,
            };

            //if (!_emailConfirmation.SendEmail(user.Email, user.Id))
            //{
            //	return ResponseMessages.UserBadEmailSyntax;
            //}

            return Result.Success(ResponseMessages.RegSuccessful);
        }

		public Result UpdateUser(UpdateUserReqDTO updateDTO, string username)
		{
			var res = _userRepository.GetUserByEmail(updateDTO.Email);

            User user = res.Value;

			if (res.IsFailure)
			    return Result.Failure(res.Description);

            bool newEmailExists = updateDTO.Email != null
                    && user.Email != updateDTO.Email
                    && _userRepository.GetUserByEmail(updateDTO.Email).IsFailure;

            if (newEmailExists) 
            {
                return Result.Failure(ResponseMessages.EmailTaken);
            }

			user.FirstName = updateDTO.FirstName ?? user.FirstName;
			user.LastName = updateDTO.LastName ?? user.LastName;
			user.Email = updateDTO.Email ?? user.Email;

            _userRepository.UpdateUser(user);

            return Result.Success(ResponseMessages.UpdateSuccessful);
		}

		public Result DeleteUser(DeleteReqDTO deleteReqDTO)
		{
			var res = _userRepository.GetUserByEmail(deleteReqDTO.Email);

			User user = res.Value;

			if (res.IsFailure)
			{
                return Result.Failure(res.Description);
			}

            return Result.Success();
		}
	}
}
