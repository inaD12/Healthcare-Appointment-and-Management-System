
using Healthcare_Appointment_and_Management_System.Extentions;
using Microsoft.AspNetCore.Http.HttpResults;
using Users.Application.Services;
using Users.Domain.DTOs.Requests;
using Users.Domain.DTOs.Responses;
using Users.Domain.Result;

namespace Healthcare_Appointment_and_Management_System.EndPoints
{
    public class UserEndPoints : IEndPoints
	{
		public void RegisterEndpoints(IEndpointRouteBuilder app)
		{
			var group = app.MapGroup("api/users");

			group.MapPost("", Login).Produces<TokenDTO>(StatusCodes.Status200OK).Produces<MessageDTO>(StatusCodes.Status404NotFound);
		}

		public async Task<IResult> Login(
			LoginReqDTO loginReqDTO,
			IUserService userService)
		{

			var res = userService.Login(loginReqDTO);

			return ControllerResponse.ParseAndReturnMessage(res);
		}
	}
}
