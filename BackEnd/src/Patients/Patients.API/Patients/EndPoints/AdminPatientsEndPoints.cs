using MediatR;
using Microsoft.AspNetCore.Mvc;
using Patients.API.Patients.Mappers;
using Patients.API.Patients.Models.Requests;
using Patients.API.Patients.Models.Responses;
using Patients.Application.Features.Patients.Commands.DeletePatient;
using Shared.API.Abstractions;
using Shared.API.Helpers;

namespace Patients.API.Patients.EndPoints;

internal class AdminPatientsEndPoints : IEndPoints
{
	public void RegisterEndpoints(IEndpointRouteBuilder app)
{
    var patientsGroup = app.MapGroup("/admin/patients/{patientId}")
        .RequireAuthorization();
    
    patientsGroup.MapPost("/allergies", AddAllergyAsync)
        .Produces<AllergyCommandResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization(Permissions.AddAllergyAdmin);

    patientsGroup.MapDelete("/allergies", RemoveAllergyAsync)
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization(Permissions.RemoveAllergyAdmin);

    patientsGroup.MapPost("/chronic-conditions", AddChronicConditionAsync)
        .Produces<ConditionCommandResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization(Permissions.AddChronicConditionAdmin);

    patientsGroup.MapDelete("/chronic-conditions", RemoveChronicConditionAsync)
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization(Permissions.RemoveChronicConditionAdmin);

    patientsGroup.MapDelete("/", DeletePatientAsync)
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization(Permissions.DeletePatient);
}

	private async Task<IResult> AddAllergyAsync(
		[FromRoute] string patientId,
		[FromBody] AddAllergyRequest request,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var command = request.ToCommand(patientId);
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);
		return ControllerResponse.ParseAndReturnMessage(res, new AllergyCommandResponse(res.Value!.Id));
	}
	
	private async Task<IResult> AddChronicConditionAsync(
		[FromRoute] string patientId,
		[FromBody] AddChronicConditionRequest request,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var command = request.ToCommand(patientId);
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);
		return ControllerResponse.ParseAndReturnMessage(res, new ConditionCommandResponse(res.Value!.Id));
	}
	
	private async Task<IResult> RemoveAllergyAsync(
		[FromRoute] string patientId,
		[FromBody] RemoveAllergyRequest request,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var command = request.ToCommand(patientId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
	
	private async Task<IResult> RemoveChronicConditionAsync(
		[FromRoute] string patientId,
		[FromBody] RemoveConditionRequest request,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var command = request.ToCommand(patientId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
	
	private async Task<IResult> DeletePatientAsync(
		[FromRoute] string patientId,
		[FromServices] ISender sender,
		CancellationToken cancellationToken)
	{
		var command = new DeletePatientCommand(patientId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
}