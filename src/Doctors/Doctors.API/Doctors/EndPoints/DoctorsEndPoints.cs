using Doctors.API.Doctors.Mappers;
using Doctors.API.Doctors.Models.Requests;
using Doctors.API.Doctors.Models.Responses;
using Doctors.Application.Features.Doctors.Queries.GetDoctorById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.API.Abstractions;
using Shared.API.Helpers;

namespace Doctors.API.Doctors.EndPoints;

public class DoctorsEndPoints  : IEndPoints
{
	public void RegisterEndpoints(IEndpointRouteBuilder app)
	{
		var group = app.MapGroup("/api/doctors/me")
			.RequireAuthorization();

		group.MapPost("", CreateDoctorAsync)
			.Produces<DoctorCommandResponse>()
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError);

		group.MapPost("/schedule/workdays", AddWorkDayScheduleAsync)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError);
		
		group.MapPut("/schedule/workdays", ChangeWorkDayScheduleAsync)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError);
		
		group.MapDelete("/schedule/workdays", RemoveWorkDayScheduleAsync)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError);
		
		group.MapPost("/availability/extra", AddExtraAvailabilityAsync)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError);
		
		group.MapPost("/availability/unavailable", AddUnavailabilityAsync)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError);
		
		var adminGroup = app.MapGroup("/api/doctors")
			.RequireAuthorization();
		
		adminGroup.MapGet("{doctorId}", GetByIdAsync)
			.Produces<DoctorQueryResponse>()
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError);
		
		adminGroup.MapGet("", GetAllAsync)
			.Produces<DoctorPaginatedQueryResponse>()
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError);
	}

	private async Task<IResult> CreateDoctorAsync(
		[FromBody] CreateDoctorRequest request,
		[FromServices] IClaimsExtractor claimsExtractor,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var userId = claimsExtractor.GetUserId();
		var command = request.ToCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var doctorCommandResponse = new DoctorCommandResponse(res.Value!.Id);
		return ControllerResponse.ParseAndReturnMessage(res, doctorCommandResponse);
	}

	private async Task<IResult> AddWorkDayScheduleAsync(
		[FromBody] AddWorkDayScheduleRequest request,
		[FromServices] IClaimsExtractor claimsExtractor,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var userId = claimsExtractor.GetUserId();
		var command = request.ToCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
	
	private async Task<IResult> ChangeWorkDayScheduleAsync(
		[FromBody] ChangeWorkDayScheduleRequest request,
		[FromServices] IClaimsExtractor claimsExtractor,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var userId = claimsExtractor.GetUserId();
		var command = request.ToCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
	
	private async Task<IResult> RemoveWorkDayScheduleAsync(
		[FromBody] RemoveWorkDayScheduleRequest request,
		[FromServices] IClaimsExtractor claimsExtractor,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var userId = claimsExtractor.GetUserId();
		var command = request.ToCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}

	private async Task<IResult> AddExtraAvailabilityAsync(
		[FromBody] AddExtraAvailabilityRequest request,
		[FromServices] IClaimsExtractor claimsExtractor,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var userId = claimsExtractor.GetUserId();
		var command = request.ToCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}

	private async Task<IResult> AddUnavailabilityAsync(
		[FromBody] AddUnavailabilityRequest request,
		[FromServices] IClaimsExtractor claimsExtractor,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var userId = claimsExtractor.GetUserId();
		var command = request.ToCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
	
	private async Task<IResult> GetByIdAsync(
		[FromRoute] string doctorId,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var command = new GetDoctorByIdQuery(doctorId);
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var queryResponse = res.Value!.ToResponse();
		return ControllerResponse.ParseAndReturnMessage(res, queryResponse);
	}
	
	private async Task<IResult> GetAllAsync(
		[AsParameters] GetAllDoctorsRequest request,
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
}
