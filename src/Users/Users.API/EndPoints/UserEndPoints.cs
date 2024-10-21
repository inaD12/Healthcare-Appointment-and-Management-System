using Healthcare_Appointment_and_Management_System.Extentions;
using Users.Application.Helpers;
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

			group.MapPost("login", Login).Produces<TokenDTO>(StatusCodes.Status200OK).Produces<MessageDTO>(StatusCodes.Status404NotFound);
			group.MapPost("register", Register).Produces<MessageDTO>(StatusCodes.Status201Created).Produces<MessageDTO>(StatusCodes.Status409Conflict);
			group.MapPost("update", Update).Produces<MessageDTO>(StatusCodes.Status200OK).Produces<MessageDTO>(StatusCodes.Status409Conflict).Produces<MessageDTO>(StatusCodes.Status404NotFound);
		}

		public  IResult Login(
			LoginReqDTO loginReqDTO,
			IUserService userService)
		{

			var res = userService.Login(loginReqDTO);

			return ControllerResponse.ParseAndReturnMessage(res);
		}
		public IResult Register(
			RegisterReqDTO registerReqDTO,
			IUserService userService)
		{

			var res = userService.Register(registerReqDTO);

			return ControllerResponse.ParseAndReturnMessage(res);
		}
		public IResult Update(
			UpdateUserReqDTO updateUserReqDTO,
			IUserService userService,
			IJwtParser jwtParser)
		{
			string id = jwtParser.GetIdFromToken();

			var res = userService.UpdateUser(updateUserReqDTO, id);

			return ControllerResponse.ParseAndReturnMessage(res);
		}
	}
}
