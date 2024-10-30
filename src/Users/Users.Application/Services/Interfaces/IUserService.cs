using FluentEmail.Core;
using Users.Domain.DTOs.Requests;
using Users.Domain.DTOs.Responses;
using Users.Domain.Result;

namespace Users.Application.Services.Interfaces
{
    public interface IUserService
    {
        Task<Result> DeleteUserAsync(string id);
        Task<Result<TokenDTO>> LoginAsync(LoginReqDTO loginDTO);
        Task<Result> RegisterAsync(RegisterReqDTO registerReqDTO);
        Task<Result> UpdateUserAsync(UpdateUserReqDTO updateDTO, string id);
    }
}