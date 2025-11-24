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

	    group.MapPost("/", RegisterAsync)
	        .Produces<UserCommandResponse>(StatusCodes.Status201Created)
	        .Produces(StatusCodes.Status400BadRequest)
	        .Produces(StatusCodes.Status409Conflict)
	        .Produces(StatusCodes.Status500InternalServerError)
	        .AllowAnonymous();

	    group.MapGet("/", GetAllAsync)
	        .Produces<UserPaginatedQueryResponse>()
	        .Produces(StatusCodes.Status401Unauthorized)
	        .Produces(StatusCodes.Status404NotFound)
	        .Produces(StatusCodes.Status500InternalServerError)
	        .RequireAuthorization(Permissions.GetUser);

	    group.MapGet("/{id}", GetByIdAsync)
	        .Produces<UserQueryResponse>()
	        .Produces(StatusCodes.Status400BadRequest)
	        .Produces(StatusCodes.Status401Unauthorized)
	        .Produces(StatusCodes.Status404NotFound)
	        .Produces(StatusCodes.Status500InternalServerError)
	        .RequireAuthorization(Permissions.GetUser);

	    group.MapPut("/{id}", UpdateAsync)
	        .Produces<UserCommandResponse>()
	        .Produces(StatusCodes.Status400BadRequest)
	        .Produces(StatusCodes.Status401Unauthorized)
	        .Produces(StatusCodes.Status404NotFound)
	        .Produces(StatusCodes.Status409Conflict)
	        .Produces(StatusCodes.Status500InternalServerError)
	        .RequireAuthorization(Permissions.ModifyUser);

	    group.MapDelete("/{id}", DeleteByIdAsync)
	        .Produces(StatusCodes.Status200OK)
	        .Produces(StatusCodes.Status401Unauthorized)
	        .Produces(StatusCodes.Status404NotFound)
	        .Produces(StatusCodes.Status500InternalServerError)
	        .RequireAuthorization(Permissions.DeleteUser);

	    group.MapGet("verify-email", VerifyEmailAsync)
	        .Produces(StatusCodes.Status200OK)
	        .Produces(StatusCodes.Status400BadRequest)
	        .Produces(StatusCodes.Status500InternalServerError)
	        .WithName("VerifyEmail")
	        .AllowAnonymous();
	    
	    var meGroup = app.MapGroup("/api/users/me");
	    
	    meGroup.MapDelete("/", DeleteCurrentAsync)
		    .Produces(StatusCodes.Status200OK)
		    .Produces(StatusCodes.Status401Unauthorized)
		    .Produces(StatusCodes.Status404NotFound)
		    .Produces(StatusCodes.Status500InternalServerError)
		    .RequireAuthorization();
	    
	    meGroup.MapPut("/", UpdateCurrentAsync)
		    .Produces<UserCommandResponse>()
		    .Produces(StatusCodes.Status400BadRequest)
		    .Produces(StatusCodes.Status401Unauthorized)
		    .Produces(StatusCodes.Status404NotFound)
		    .Produces(StatusCodes.Status409Conflict)
		    .Produces(StatusCodes.Status500InternalServerError)
		    .RequireAuthorization();
	}

	private async Task<IResult> RegisterAsync(
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

	private async Task<IResult> UpdateCurrentAsync(
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

	private async Task<IResult> DeleteCurrentAsync(
		HttpContext httpContext,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = new DeleteUserCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}

	private async Task<IResult> VerifyEmailAsync(
		[FromBody] VerifyEmailRequest request,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var command = request.ToCommand();
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
}