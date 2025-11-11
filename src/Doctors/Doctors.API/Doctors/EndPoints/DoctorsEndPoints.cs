using Doctors.API.Doctors.Mappers;
using Doctors.API.Doctors.Models.Requests;
using Doctors.API.Doctors.Models.Responses;
using Doctors.Application.Features.Doctors.Queries.GetDoctorById;
using Doctors.Application.Features.Doctors.Queries.GetDoctorByUserId;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.API.Abstractions;
using Shared.API.Helpers;
using Shared.Infrastructure.Authentication;

namespace Doctors.API.Doctors.EndPoints;

public class DoctorsEndPoints  : IEndPoints
{
	public void RegisterEndpoints(IEndpointRouteBuilder app)
	{
		var specialitiesGroup = app.MapGroup("/api/specialities");
		
		specialitiesGroup.MapPost("/recommend", RecommendSpecialityAsync)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.RequestRecommendations);
		
		var meGroup = app.MapGroup("/api/doctors/me");

		meGroup.MapPost("", CreateDoctorAsync)
			.Produces<DoctorCommandResponse>()
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.CreateDoctor);
		
		meGroup.MapPut("", UpdateDoctorInfoAsync)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.UpdateDoctor);
		
		meGroup.MapGet("", GetDoctorAsync)
			.Produces<DoctorQueryResponse>()
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.ViewDoctor);
		
		meGroup.MapPost("/specialities", AddSpecialityAsync)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.AddSpeciality);
		
		meGroup.MapDelete("/specialities", DeleteSpecialityAsync)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.RemoveSpeciality);

		meGroup.MapPost("/schedule/workdays", AddWorkDayScheduleAsync)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.AddWorkDaySchedule);
		
		meGroup.MapPut("/schedule/workdays", ChangeWorkDayScheduleAsync)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.ChangeWorkDaySchedule);
		
		meGroup.MapDelete("/schedule/workdays", RemoveWorkDayScheduleAsync)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.RemoveWorkDaySchedule);
		
		meGroup.MapPost("/availability/extra", AddExtraAvailabilityAsync)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.AddExtraAvailability);
		
		meGroup.MapPost("/availability/unavailable", AddUnavailabilityAsync)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.AddUnavailability);
		
		meGroup.MapDelete("/availability/extra", DeleteExtraAvailabilityAsync)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.RemoveExtraAvailability);
		
		meGroup.MapDelete("/availability/unavailable", DeleteUnavailabilityAsync)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.RemoveUnavailability);

		var adminGroup = app.MapGroup("/api/doctors");
		
		adminGroup.MapPost("", CreateDoctorByAdminAsync)
			.Produces<DoctorCommandResponse>()
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.CreateDoctorByAdmin);
		
		adminGroup.MapPut("", UpdateDoctorInfoByAdminAsync)
			.Produces(StatusCodes.Status200OK)
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.UpdateDoctorByAdmin);
		
		adminGroup.MapGet("/by-id/{doctorId}", GetDoctorByIdAsync)
			.Produces<DoctorQueryResponse>()
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.ViewDoctorByAdmin);

		adminGroup.MapGet("/by-user/{userId}", GetDoctorByUserIdAsync)
			.Produces<DoctorQueryResponse>()
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.ViewDoctorByAdmin);

		adminGroup.MapGet("", GetAllDoctorsAsync)
			.Produces<DoctorPaginatedQueryResponse>()
			.Produces(StatusCodes.Status400BadRequest)
			.Produces(StatusCodes.Status401Unauthorized)
			.Produces(StatusCodes.Status404NotFound)
			.Produces(StatusCodes.Status409Conflict)
			.Produces(StatusCodes.Status500InternalServerError)
			.RequireAuthorization(Permissions.ViewAllDoctors);
	}

	private async Task<IResult> RecommendSpecialityAsync(
		[FromBody] RecommendSpecialityRequest request,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var command = request.ToCommand();
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var recommendSpecialityResponse = new RecommendSpecialityResponse(res.Value!);
		return ControllerResponse.ParseAndReturnMessage(res, recommendSpecialityResponse);
	}
	
	private async Task<IResult> CreateDoctorAsync(
		[FromBody] CreateDoctorRequest request,
		HttpContext httpContext,   
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = request.ToCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var doctorCommandResponse = new DoctorCommandResponse(res.Value!.Id);
		return ControllerResponse.ParseAndReturnMessage(res, doctorCommandResponse);
	}
	
	private async Task<IResult> CreateDoctorByAdminAsync(
		[FromBody] CreateDoctorByAdminRequest request,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var command = request.ToCommand();
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		return ControllerResponse.ParseAndReturnMessage(res, new DoctorCommandResponse(res.Value!.Id));
	}
	
	private async Task<IResult> UpdateDoctorInfoAsync(
		[FromBody] UpdateDoctorInfoRequest request,
		HttpContext httpContext,   
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = request.ToCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
	
	private async Task<IResult> UpdateDoctorInfoByAdminAsync(
		[FromBody] UpdateDoctorInfoByAdminRequest request,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var command = request.ToCommand();
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
	
	private async Task<IResult> GetDoctorAsync(
		HttpContext httpContext,   
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = new GetDoctorByUserIdQuery(userId);
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var queryResponse = res.Value!.ToResponse();
		return ControllerResponse.ParseAndReturnMessage(res, queryResponse);
	}
	
	private async Task<IResult> AddSpecialityAsync(
		[FromBody] AddSpecialityRequest request,
		HttpContext httpContext,   
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = request.ToCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
	
	private async Task<IResult> DeleteSpecialityAsync(
		[FromBody] RemoveSpecialityRequest request,
		HttpContext httpContext,   
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = request.ToCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
	
	private async Task<IResult> AddWorkDayScheduleAsync(
		[FromBody] AddWorkDayScheduleRequest request,
		HttpContext httpContext,   
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = request.ToCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
	
	private async Task<IResult> ChangeWorkDayScheduleAsync(
		[FromBody] ChangeWorkDayScheduleRequest request,
		HttpContext httpContext,   
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = request.ToCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
	
	private async Task<IResult> RemoveWorkDayScheduleAsync(
		[FromBody] RemoveWorkDayScheduleRequest request,
		HttpContext httpContext,   
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = request.ToCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}

	private async Task<IResult> AddExtraAvailabilityAsync(
		[FromBody] AddExtraAvailabilityRequest request,
		HttpContext httpContext,   
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = request.ToCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}

	private async Task<IResult> AddUnavailabilityAsync(
		[FromBody] AddUnavailabilityRequest request,
		HttpContext httpContext,   
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = request.ToCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
	
	private async Task<IResult> DeleteExtraAvailabilityAsync(
		[FromBody] RemoveExtraAvailabilityRequest request,
		HttpContext httpContext,   
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = request.ToCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}

	private async Task<IResult> DeleteUnavailabilityAsync(
		[FromBody] RemoveUnavailabilityRequest request,
		HttpContext httpContext,   
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = request.ToCommand(userId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
	
	private async Task<IResult> GetDoctorByIdAsync(
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
	
	private async Task<IResult> GetDoctorByUserIdAsync(
		[FromRoute] string userId,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var command = new GetDoctorByUserIdQuery(userId);
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);

		var queryResponse = res.Value!.ToResponse();
		return ControllerResponse.ParseAndReturnMessage(res, queryResponse);
	}
	
	private async Task<IResult> GetAllDoctorsAsync(
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
