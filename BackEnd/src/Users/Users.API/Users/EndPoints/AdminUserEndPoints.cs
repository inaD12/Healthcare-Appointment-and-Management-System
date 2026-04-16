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

internal class AdminUserEndPoints : IEndPoints
{
	public void RegisterEndpoints(IEndpointRouteBuilder app)
	{
	    var adminGroup = app.MapGroup("/admin/users");

	    adminGroup.MapPost("/", CreateAsync)
	        .Produces<UserCommandResponse>(StatusCodes.Status201Created)
	        .Produces(StatusCodes.Status400BadRequest)
	        .Produces(StatusCodes.Status409Conflict)
	        .Produces(StatusCodes.Status500InternalServerError)
	        .RequireAuthorization(Permissions.CreateUser);

	    adminGroup.MapGet("/", GetAllAsync)
	        .Produces<UserPaginatedQueryResponse>()
	        .Produces(StatusCodes.Status401Unauthorized)
	        .Produces(StatusCodes.Status404NotFound)
	        .Produces(StatusCodes.Status500InternalServerError)
	        .RequireAuthorization(Permissions.GetUser);

	    adminGroup.MapGet("/{id}", GetByIdAsync)
	        .Produces<UserQueryResponse>()
	        .Produces(StatusCodes.Status400BadRequest)
	        .Produces(StatusCodes.Status401Unauthorized)
	        .Produces(StatusCodes.Status404NotFound)
	        .Produces(StatusCodes.Status500InternalServerError)
	        .RequireAuthorization(Permissions.GetUser);

	    adminGroup.MapPut("/{id}", UpdateAsync)
	        .Produces<UserCommandResponse>()
	        .Produces(StatusCodes.Status400BadRequest)
	        .Produces(StatusCodes.Status401Unauthorized)
	        .Produces(StatusCodes.Status404NotFound)
	        .Produces(StatusCodes.Status409Conflict)
	        .Produces(StatusCodes.Status500InternalServerError)
	        .RequireAuthorization(Permissions.ModifyUser);

	    adminGroup.MapDelete("/{id}", DeleteByIdAsync)
	        .Produces(StatusCodes.Status200OK)
	        .Produces(StatusCodes.Status401Unauthorized)
	        .Produces(StatusCodes.Status404NotFound)
	        .Produces(StatusCodes.Status500InternalServerError)
	        .RequireAuthorization(Permissions.DeleteUser);
	}

	private async Task<IResult> CreateAsync(
		[FromBody] RegisterUserByAdminRequest request,
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
	private async Task<IResult> UpdateAsync(
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
	private async Task<IResult> GetByIdAsync(
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
	private async Task<IResult> GetAllAsync(
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

	private async Task<IResult> DeleteByIdAsync(
		[FromRoute] string id,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var command = new DeleteUserCommand(id);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
}