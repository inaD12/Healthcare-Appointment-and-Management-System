using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.API.Abstractions;
using Shared.API.Helpers;
using Shared.Application.Abstractions;
using Shared.Utilities;
using Users.Application.Features.Email.Commands.HandleEmail;
using Users.Application.Features.Users.Commands.DeleteUser;
using Users.Application.Features.Users.Commands.RegisterUser;
using Users.Application.Features.Users.LoginUser;
using Users.Application.Features.Users.Queries.GetAllUsers;
using Users.Application.Features.Users.Queries.GetById;
using Users.Application.Features.Users.UpdateUser;
using Users.Users.Models.Requests;
using Users.Users.Models.Responses;

namespace Users.Users.EndPoints;

internal class UserEndPoints : IEndPoints
{
	public void RegisterEndpoints(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/users");

		group.MapPost("login", Login)
			.Produces<LoginUserResponse>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status500InternalServerError)
			.AllowAnonymous();

		group.MapPost("register", Register)
			.Produces<UserCommandResponse>(StatusCodes.Status201Created)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.AllowAnonymous();

		group.MapPut("update-current", UpdateCurrent)
			.Produces<UserCommandResponse>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization();

		group.MapPut("update/{id}", Update)
			.Produces<UserCommandResponse>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError);

		group.MapGet("get-all", GetAll)
			.Produces<UserPaginatedQueryResponse>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status500InternalServerError);
		//.RequireAuthorization();

		group.MapGet("get/{id}", GetById)
			.Produces<UserQueryResponse>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status500InternalServerError);
		//.RequireAuthorization();

		group.MapDelete("delete-current", DeleteCurrent)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization();

		group.MapDelete("delete/{id}", DeleteById)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status500InternalServerError);
		//.RequireAuthorization();

		group.MapGet("verify-email", VerifyEmails)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status500InternalServerError)
			.WithName("VerifyEmail");
	}

	public async Task<IResult> Login(
		[FromBody] LoginUserRequest request,
		[FromServices] ISender sender,
		[FromServices] IHAMSMapper mapper,
		CancellationToken cancellationToken)
	{
		var command = mapper.Map<LoginUserCommand>(request);
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var loginUserResponse = mapper.Map<LoginUserResponse>(res.Value!);
		return ControllerResponse.ParseAndReturnMessage(res, loginUserResponse);
	}

	public async Task<IResult> Register(
		[FromBody] RegisterUserRequest request,
		[FromServices] ISender sender,
		[FromServices] IHAMSMapper mapper,
		CancellationToken cancellationToken)
	{
		var command = mapper.Map<RegisterUserCommand>(request);
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var userCommandResponse = mapper.Map<UserCommandResponse>(res.Value!);
		return ControllerResponse.ParseAndReturnMessage(res, userCommandResponse);
	}

	public async Task<IResult> UpdateCurrent(
		[FromBody] UpdateCurrentUserRequest request,
		[FromServices] IClaimsExtractor claimsExtractor,
		[FromServices] ISender sender,
		[FromServices] IHAMSMapper mapper,
		CancellationToken cancellationToken)
	{
		var userId = claimsExtractor.GetUserId();
		var command = mapper.Map<UpdateUserCommand>((request, userId));
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var userCommandResponse = mapper.Map<UserCommandResponse>(res.Value!);
		return ControllerResponse.ParseAndReturnMessage(res, userCommandResponse);
	}

	public async Task<IResult> Update(
		[FromRoute] string id,
		[FromBody] UpdateUserRequest request,
		[FromServices] ISender sender,
		[FromServices] IHAMSMapper mapper,
		CancellationToken cancellationToken)
	{
		var command = mapper.Map<UpdateUserCommand>((request, id));
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var userCommandResponse = mapper.Map<UserCommandResponse>(res.Value!);
		return ControllerResponse.ParseAndReturnMessage(res, userCommandResponse);
	}

	public async Task<IResult> GetById(
		[FromRoute] string id,
		[FromServices] ISender sender,
		[FromServices] IHAMSMapper mapper,
		CancellationToken cancellationToken)
	{
		var query = mapper.Map<GetUserByIdQuery>(id);
		var res = await sender.Send(query, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var appointmentCommandResponse = mapper.Map<UserQueryResponse>(res.Value!);
		return ControllerResponse.ParseAndReturnMessage(res, appointmentCommandResponse);
	}
	public async Task<IResult> GetAll(
		[AsParameters] GetAllUsersRequest request,
		[FromServices] ISender sender,
		[FromServices] IHAMSMapper mapper,
		CancellationToken cancellationToken)
	{
		var query = mapper.Map<GetAllUsersQuery>(request);
		var res = await sender.Send(query, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var userCommandResponse = mapper.Map<UserPaginatedQueryResponse>(res.Value!);
		return ControllerResponse.ParseAndReturnMessage(res, userCommandResponse);
	}

	public async Task<IResult> DeleteById(
		[FromRoute] string id,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var command = new DeleteUserCommand(id);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}

	public async Task<IResult> DeleteCurrent(
		[FromServices] IClaimsExtractor claimsExtractor,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var userId = claimsExtractor.GetUserId();
		var command = new DeleteUserCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}

	public async Task<IResult> VerifyEmails(
		[FromBody] VerifyEmailRequest request,
		[FromServices] ISender sender,
		[FromServices] IHAMSMapper mapper,
		CancellationToken cancellationToken)
	{
		var command = mapper.Map<HandleEmailCommand>(request);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
}