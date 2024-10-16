using Users.Domain.DTOs.Requests;
using Users.Domain.Result;

namespace Users.Application.Services
{
	public interface IUserService
	{
		Result DeleteUser(DeleteReqDTO deleteReqDTO);
		Result<string> Login(LoginReqDTO loginDTO);
		Result Register(RegisterReqDTO registerReqDTO);
		Result UpdateUser(UpdateUserReqDTO updateDTO, string id);
	}
}