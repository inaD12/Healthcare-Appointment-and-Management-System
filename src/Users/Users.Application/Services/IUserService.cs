using Users.Domain.DTOs.Requests;
using Users.Domain.DTOs.Responses;
using Users.Domain.Result;

namespace Users.Application.Services
{
	public interface IUserService
	{
		Result DeleteUser(string id);
		Result<TokenDTO> Login(LoginReqDTO loginDTO);
		Result Register(RegisterReqDTO registerReqDTO);
		Result UpdateUser(UpdateUserReqDTO updateDTO, string id);
	}
}