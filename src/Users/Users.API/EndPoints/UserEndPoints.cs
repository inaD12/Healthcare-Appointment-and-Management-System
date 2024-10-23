using FluentValidation;
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

			group.MapPost("login", Login)
				.Produces<TokenDTO>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status400BadRequest)
				.Produces<MessageDTO>(StatusCodes.Status404NotFound)
				.Produces(StatusCodes.Status500InternalServerError)
				.AllowAnonymous();

			group.MapPost("register", Register)
				.Produces<MessageDTO>(StatusCodes.Status201Created)
				.Produces(StatusCodes.Status400BadRequest)
				.Produces<MessageDTO>(StatusCodes.Status409Conflict)
				.Produces(StatusCodes.Status500InternalServerError)
				.AllowAnonymous();

			group.MapPut("update", Update)
				.Produces<MessageDTO>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status400BadRequest)
				.Produces(StatusCodes.Status401Unauthorized)
				.Produces<MessageDTO>(StatusCodes.Status404NotFound)
				.Produces<MessageDTO>(StatusCodes.Status409Conflict)
				.Produces(StatusCodes.Status500InternalServerError)
				.RequireAuthorization();

			group.MapDelete("delete", Delete)
				.Produces<MessageDTO>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status401Unauthorized)
				.Produces<MessageDTO>(StatusCodes.Status404NotFound)
				.Produces(StatusCodes.Status500InternalServerError)
				.RequireAuthorization();
		}

		private async Task<IResult?> ValidateAndReturnResponse<T>(T dto, IValidator<T> validator)
		{
			var valResult = await validator.ValidateAsync(dto);

			if (!valResult.IsValid)
			{
				var errors = valResult.Errors.Select(e => e.ErrorMessage).ToList();
				return Results.BadRequest(new { Errors = errors });
			}

			return null;
		}

		public async Task<IResult> Login(
			LoginReqDTO loginReqDTO,
			IUserService userService,
			IValidator<LoginReqDTO> validator)
		{
			var validationResponse = await ValidateAndReturnResponse(loginReqDTO, validator);
			if (validationResponse != null)
				return validationResponse;

			var res = userService.Login(loginReqDTO);

			return ControllerResponse.ParseAndReturnMessage(res);
		}

		public async Task<IResult> Register(
			RegisterReqDTO registerReqDTO,
			IUserService userService,
			IValidator<RegisterReqDTO> validator)
		{
			var validationResponse = await ValidateAndReturnResponse(registerReqDTO, validator);
			if (validationResponse != null)
				return validationResponse;

			var res = userService.Register(registerReqDTO);

			return ControllerResponse.ParseAndReturnMessage(res);
		}

		public async Task<IResult> Update(
			UpdateUserReqDTO updateUserReqDTO,
			IUserService userService,
			IJwtParser jwtParser,
			IValidator<UpdateUserReqDTO> validator)
		{
			var validationResponse = await ValidateAndReturnResponse(updateUserReqDTO, validator);
			if (validationResponse != null)
				return validationResponse;

			string id = jwtParser.GetIdFromToken();

			var res = userService.UpdateUser(updateUserReqDTO, id);

			return ControllerResponse.ParseAndReturnMessage(res);
		}

		public async Task<IResult> Delete(
			IUserService userService,
			IJwtParser jwtParser)
		{
			string id = jwtParser.GetIdFromToken();

			var res = userService.DeleteUser(id);

			return ControllerResponse.ParseAndReturnMessage(res);
		}
	}
}