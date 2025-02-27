using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.API.Abstractions;
using Shared.API.Helpers;
using Shared.Application.Abstractions;
using Shared.Application.Helpers.Abstractions;
using Users.Application.Features.Auth.Models;
using Users.Application.Features.Email.Commands.HandleEmail;
using Users.Application.Features.Users.Commands.DeleteUser;
using Users.Application.Features.Users.Commands.RegisterUser;
using Users.Application.Features.Users.LoginUser;
using Users.Application.Features.Users.Queries.GetAllDoctors;
using Users.Application.Features.Users.UpdateUser;
using Users.Domain.Entities;
using Users.Users.Models.Requests;

namespace Users.Users.EndPoints;

internal class UserEndPoints : IEndPoints
{
	public void RegisterEndpoints(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/users");

		group.MapPost("login", Login)
			.Produces<TokenResult>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status500InternalServerError)
			.AllowAnonymous();

		group.MapPost("register", Register)
			.Produces(StatusCodes.Status201Created)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.AllowAnonymous();

		group.MapPut("update-current", UpdateCurrent)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization();

		group.MapGet("get-all-doctors", GetAllDoctors)
			.Produces<IEnumerable<User>>(StatusCodes.Status200OK)
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

		group.MapGet("verify-email", VerifyEmails)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status500InternalServerError)
			.WithName("VerifyEmail");
	}

	public async Task<IResult> Login(
		[FromBody] LoginUserRequest request,
		[FromServices] ISender sender,
		[FromServices] IHAMSMapper hamsMapper,
		CancellationToken cancellationToken)
	{
		var command = hamsMapper.Map<LoginUserCommand<TokenResult>>(request);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}

	public async Task<IResult> Register(
		[FromBody] RegisterUserRequest request,
		[FromServices] ISender sender,
		[FromServices] IHAMSMapper hamsMapper,
		CancellationToken cancellationToken)
	{
		var command = hamsMapper.Map<RegisterUserCommand>(request);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}

	public async Task<IResult> UpdateCurrent(
		[FromBody] UpdateCurrentUserRequest request,
		[FromServices] ISender sender,
		[FromServices] IHAMSMapper hamsMapper,
		[FromServices] IJwtParser jwtParser,
		CancellationToken cancellationToken)
	{
		var parserRes = jwtParser.GetIdFromToken();
		if (parserRes.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(parserRes);
		string id = parserRes.Value!;

		var command = hamsMapper.Map<UpdateUserCommand>((id,request));
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}

	public async Task<IResult> GetAllDoctors(
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var query = new GetAllDoctorsQuery();
		var res = await sender.Send(query, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}

	public async Task<IResult> DeleteCurrent(
		[FromServices] ISender sender,
		[FromServices] IJwtParser jwtParser,
		CancellationToken cancellationToken)
	{
		var parserRes = jwtParser.GetIdFromToken();
		if (parserRes.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(parserRes);
		string id = parserRes.Value!;

		var command = new DeleteUserCommand(id);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}

	public async Task<IResult> VerifyEmails(
		[FromBody] VerifyEmailRequest request,
		[FromServices] ISender sender,
		[FromServices] IHAMSMapper hamsMapper,
		CancellationToken cancellationToken)
	{
		var command = hamsMapper.Map<HandleEmailCommand>(request);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
}