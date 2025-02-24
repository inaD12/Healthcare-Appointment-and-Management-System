﻿using FluentValidation;
using MediatR;
using Shared.API.Abstractions;
using Shared.API.Helpers;
using Shared.Application.Helpers.Abstractions;
using Users.Application.Features.Auth.Models;
using Users.Application.Features.Email.Commands.HandleEmail;
using Users.Application.Features.Users.Commands.DeleteUser;
using Users.Application.Features.Users.Commands.RegisterUser;
using Users.Application.Features.Users.LoginUser;
using Users.Application.Features.Users.Queries.GetAllDoctors;
using Users.Application.Features.Users.UpdateUser;
using Users.Domain.DTOs.Requests;
using Users.Domain.Entities;

namespace Users.EndPoints;

internal class UserEndPoints : IEndPoints
{
	public void RegisterEndpoints(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("api/users");

		group.MapPost("login", Login)
			.Produces<TokenResult>(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status500InternalServerError)
			.AllowAnonymous();

		group.MapPost("register", Register)
			.Produces(StatusCodes.Status201Created)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.AllowAnonymous();

		group.MapPut("update", Update)
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

		group.MapDelete("delete", Delete)
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
		LoginReqDTO loginReqDTO,
		ISender sender,
		CancellationToken cancellationToken)
	{
		var command = new LoginUserCommand<TokenResult>(
			loginReqDTO.Email,
			loginReqDTO.Password);

		var res = await sender.Send(command, cancellationToken);

		return ControllerResponse.ParseAndReturnMessage(res);
	}

	public async Task<IResult> Register(
		RegisterReqDTO registerReqDTO,
		ISender sender,
		CancellationToken cancellationToken)
	{
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
		CancellationToken cancellationToken)
	{
		var parserRes = jwtParser.GetIdFromToken();
		if (parserRes.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(parserRes);

		string id = parserRes.Value!;

		var command = new UpdateUserCommand(
			id,
			updateUserReqDTO.NewEmail,
			updateUserReqDTO.FirstName,
			updateUserReqDTO.LastName);

		var res = await sender.Send(command, cancellationToken);

		return ControllerResponse.ParseAndReturnMessage(res);
	}

	public async Task<IResult> GetAllDoctors(
		ISender sender,
		CancellationToken cancellationToken)
	{
		var query = new GetAllDoctorsQuery();

		var res = await sender.Send(query, cancellationToken);

		return ControllerResponse.ParseAndReturnMessage(res);
	}

	public async Task<IResult> Delete(
		ISender sender,
		IJwtParser jwtParser,
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
		string token,
		ISender sender,
		CancellationToken cancellationToken)
	{
		var command = new HandleEmailCommand(token);

		var res = await sender.Send(command, cancellationToken);

		return ControllerResponse.ParseAndReturnMessage(res);
	}
}