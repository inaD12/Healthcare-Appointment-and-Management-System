using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.API.Abstractions;
using Shared.API.Helpers;
using Shared.Domain.Entities;
using Shared.Infrastructure.Authentication;
using Users.Application.Features.Users.Commands.DeleteUser;
using Users.Application.Features.Users.Commands.RegisterUserByAdmin;
using Users.Application.Features.Users.Queries.GetUserById;
using Users.Users.Mappers;
using Users.Users.Models.Requests;
using Users.Users.Models.Responses;

namespace Users.Users.EndPoints;

internal class UserEndPoints : IEndPoints
{
	public void RegisterEndpoints(IEndpointRouteBuilder app)
	{
	    var group = app.MapGroup("/users");

	    group.MapPost("/", RegisterAsync)
	        .Produces<UserCommandResponse>(StatusCodes.Status201Created)
	        .Produces(StatusCodes.Status400BadRequest)
	        .Produces(StatusCodes.Status409Conflict)
	        .Produces(StatusCodes.Status500InternalServerError)
	        .AllowAnonymous();

	    group.MapGet("verify-email", VerifyEmailAsync)
	        .Produces(StatusCodes.Status200OK)
	        .Produces(StatusCodes.Status400BadRequest)
	        .Produces(StatusCodes.Status500InternalServerError)
	        .WithName("VerifyEmail")
	        .AllowAnonymous();
	    
	    var meGroup = app.MapGroup("/users/me");
	    
	    meGroup.MapGet("/", GetCurrentAsync)
		    .Produces<UserQueryResponse>()
		    .Produces(StatusCodes.Status401Unauthorized)
		    .Produces(StatusCodes.Status404NotFound)
		    .Produces(StatusCodes.Status500InternalServerError)
		    .RequireAuthorization();
	    
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
	
	private async Task<IResult> GetCurrentAsync(
		HttpContext httpContext,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var query = new GetUserByIdQuery(userId);
		var res = await sender.Send(query, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var appointmentCommandResponse = res.Value!.ToResponse();
		return ControllerResponse.ParseAndReturnMessage(res, appointmentCommandResponse);
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