
using Healthcare_Appointment_and_Management_System.Extentions;
using Microsoft.AspNetCore.Http.HttpResults;
using Users.Application.Services;
using Users.Domain.DTOs.Requests;
using Users.Domain.DTOs.Responses;

namespace Healthcare_Appointment_and_Management_System.EndPoints
{
	public class UserEndPoints : IEndPoints
	{
		public void RegisterEndpoints(IEndpointRouteBuilder app)
		{
			var group = app.MapGroup("api/users");

			group.MapPost("", Login);
		}

		public async Task<Results<Ok<TokenDTO>, NotFound<string>>> Login(
			LoginReqDTO loginReqDTO,
			IUserService userService)
		{

			var res = userService.Login(loginReqDTO);

			return (Results<Ok<TokenDTO>, NotFound<string>>)ControllerResponse.ParseAndReturnMessage(res);

		}
	}
}
