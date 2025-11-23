using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.API.Abstractions;
using Shared.API.Helpers;
using Shared.Infrastructure.Authentication;
using Users.Application.Features.Users.Commands.DeleteUser;
using Users.Application.Features.Users.Queries.GetUserById;
using Users.Users.Mappers;
using Users.Users.Models.Requests;
using Users.Users.Models.Responses;

namespace Users.Users.EndPoints;

internal class UserEndPoints : IEndPoints
{
	public void RegisterEndpoints(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/users");

		group.MapPost("register", Register)
			.Produces<UserCommandResponse>(StatusCodes.Status201Created)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.AllowAnonymous();

		group.MapPut("update-current", UpdateCurrent)
			.Produces<UserCommandResponse>()
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization();

		group.MapPut("update/{id}", Update)
			.Produces<UserCommandResponse>()
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.ModifyUser);

		group.MapGet("get-all", GetAll)
			.Produces<UserPaginatedQueryResponse>()
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.GetUser);

		group.MapGet("get/{id}", GetById)
			.Produces<UserQueryResponse>()
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.GetUser);

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
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.DeleteUser);

		group.MapGet("verify-email", VerifyEmails)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status500InternalServerError)
			.WithName("VerifyEmail")
			.AllowAnonymous();
	}

	private async Task<IResult> Register(
		[FromBody] RegisterUserRequest request,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)

	{
		var command = request.ToCommand();
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var userCommandResponse = res.Value!.ToResponse();
		return ControllerResponse.ParseAndReturnMessage(res, userCommandResponse);
	}

	private async Task<IResult> UpdateCurrent(
		[FromBody] UpdateCurrentUserRequest request,
		[FromServices] ISender sender,
		HttpContext httpContext,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = request.ToCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var userCommandResponse = res.Value!.ToResponse();
		return ControllerResponse.ParseAndReturnMessage(res, userCommandResponse);
	}

	private async Task<IResult> Update(
		[FromRoute] string id,
		[FromBody] UpdateUserRequest request,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var command = request.ToCommand(id);
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var userCommandResponse = res.Value!.ToResponse();
		return ControllerResponse.ParseAndReturnMessage(res, userCommandResponse);
	}

	private async Task<IResult> GetById(
		[FromRoute] string id,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var query = new GetUserByIdQuery(id);
		var res = await sender.Send(query, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var appointmentCommandResponse = res.Value!.ToResponse();
		return ControllerResponse.ParseAndReturnMessage(res, appointmentCommandResponse);
	}
	private async Task<IResult> GetAll(
		[AsParameters] GetAllUsersRequest request,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var query = request.ToQuery();
		var res = await sender.Send(query, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var userCommandResponse = res.Value!.ToResponse();
		return ControllerResponse.ParseAndReturnMessage(res, userCommandResponse);
	}

	private async Task<IResult> DeleteById(
		[FromRoute] string id,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var command = new DeleteUserCommand(id);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}

	private async Task<IResult> DeleteCurrent(
		HttpContext httpContext,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = new DeleteUserCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}

	private async Task<IResult> VerifyEmails(
		[FromBody] VerifyEmailRequest request,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var command = request.ToCommand();
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
}