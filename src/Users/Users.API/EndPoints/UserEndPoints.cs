using FluentValidation;
using UsersAPI.Extentions;
using Users.Application.Helpers.Interfaces;
using Users.Domain.DTOs.Requests;
using Users.Domain.DTOs.Responses;
using Users.Application.Commands.Users.LoginUser;
using MediatR;
using Users.Application.Commands.Users.RegisterUser;
using Users.Application.Commands.Users.UpdateUser;
using Users.Application.Commands.Users.DeleteUser;
using System.Threading;
using Users.Application.Commands.Email.HandleEmail;

namespace UsersAPI.EndPoints
{
    internal class UserEndPoints : IEndPoints
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

			group.MapGet("verify-email", VerifyEmails)
				.Produces<MessageDTO>(StatusCodes.Status200OK)
				.Produces(StatusCodes.Status400BadRequest)
				.Produces(StatusCodes.Status500InternalServerError)
				.WithName("VerifyEmail");
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
			ISender sender,
			IValidator<LoginReqDTO> validator,
			CancellationToken cancellationToken)
		{
			var validationResponse = await ValidateAndReturnResponse(loginReqDTO, validator);
			if (validationResponse != null)
				return validationResponse;

			var command = new LoginUserCommand<TokenDTO>(
				loginReqDTO.Email,
				loginReqDTO.Password);

			var res = await sender.Send(command, cancellationToken);

			return ControllerResponse.ParseAndReturnMessage(res);
		}

		public async Task<IResult> Register(
			RegisterReqDTO registerReqDTO,
			ISender sender,
			IValidator<RegisterReqDTO> validator,
			CancellationToken cancellationToken)
		{
			var validationResponse = await ValidateAndReturnResponse(registerReqDTO, validator);
			if (validationResponse != null)
				return validationResponse;

			var command = new RegisterUserCommand(
				registerReqDTO.Email,
				registerReqDTO.Password,
				registerReqDTO.FirstName,
				registerReqDTO.LastName,
				registerReqDTO.DateOfBirth,
				registerReqDTO.PhoneNumber,
				registerReqDTO.Address,
				registerReqDTO.Role);

			var res = await sender.Send(command, cancellationToken);

			return ControllerResponse.ParseAndReturnMessage(res);
		}

		public async Task<IResult> Update(
			UpdateUserReqDTO updateUserReqDTO,
			ISender sender,
			IJwtParser jwtParser,
			IValidator<UpdateUserReqDTO> validator,
			CancellationToken cancellationToken)
		{
			var validationResponse = await ValidateAndReturnResponse(updateUserReqDTO, validator);
			if (validationResponse != null)
				return validationResponse;

			string id = jwtParser.GetIdFromToken();

			var command = new UpdateUserCommand(
				id,
				updateUserReqDTO.NewEmail,
				updateUserReqDTO.FirstName,
				updateUserReqDTO.LastName);

			var res = await sender.Send(command, cancellationToken);

			return ControllerResponse.ParseAndReturnMessage(res);
		}

		public async Task<IResult> Delete(
			ISender sender,
			IJwtParser jwtParser,
			CancellationToken cancellationToken)
		{
			string id = jwtParser.GetIdFromToken();

			var command = new DeleteUserCommand(id);

			var res = await sender.Send(command, cancellationToken);

			return ControllerResponse.ParseAndReturnMessage(res);
		}

		public async Task<IResult> VerifyEmails(
			string token,
			ISender sender,
			CancellationToken cancellationToken)
		{
			var command = new HandleEmailCommand(token);

			var res = await sender.Send(command, cancellationToken);

			return ControllerResponse.ParseAndReturnMessage(res);
		} 
	}
}