using MediatR;
using Microsoft.AspNetCore.Mvc;
using Patients.API.Patients.Mappers;
using Patients.API.Patients.Models.Requests;
using Patients.API.Patients.Models.Responses;
using Patients.Application.Features.Encounters.Commands.LockEncounter;
using Patients.Application.Features.Patients.Commands.DeletePatient;
using Shared.API.Abstractions;
using Shared.API.Helpers;
using Shared.Infrastructure.Authentication;
using FinalizeEncounterCommand = Patients.Application.Features.Encounters.Commands.FinalizeEncounter.FinalizeEncounterCommand;

namespace Patients.API.Patients.EndPoints;

internal class PatientsEndPoints : IEndPoints
{
	public void RegisterEndpoints(IEndpointRouteBuilder app)
{
    var patientsGroup = app.MapGroup("/api/patients/{patientId}")
        .RequireAuthorization();
    
    patientsGroup.MapPost("/allergies", AddAllergyAsync)
        .Produces<AllergyCommandResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization(Permissions.AddAllergy);

    patientsGroup.MapDelete("/allergies", RemoveAllergyAsync)
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization(Permissions.RemoveAllergy);

    patientsGroup.MapPost("/chronic-conditions", AddChronicConditionAsync)
        .Produces<ConditionCommandResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization(Permissions.AddChronicCondition);

    patientsGroup.MapDelete("/chronic-conditions", RemoveChronicConditionAsync)
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization(Permissions.RemoveChronicCondition);

    patientsGroup.MapDelete("/", DeletePatientAsync)
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization(Permissions.DeletePatient);

    
    var encountersGroup = app.MapGroup("/api/encounters");
    
    encountersGroup.MapPost("/", StartEncounterAsync)
        .Produces<EncounterCommandResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization(Permissions.StartEncounter);
    
    
    var encounterActionsGroup = encountersGroup.MapGroup("/{encounterId}")
	    .RequireAuthorization();

    encounterActionsGroup.MapPost("/notes", AddNoteAsync)
        .Produces<NoteCommandResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization(Permissions.AddNote);

    encounterActionsGroup.MapDelete("/notes", RemoveNoteAsync)
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization(Permissions.RemoveNote);

    encounterActionsGroup.MapPost("/diagnoses", AddDiagnosisAsync)
        .Produces<DiagnosisCommandResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization(Permissions.AddDiagnosis);

    encounterActionsGroup.MapDelete("/diagnoses", RemoveDiagnosisAsync)
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization(Permissions.RemoveDiagnosis);

    encounterActionsGroup.MapPost("/prescriptions", PrescribeMedicationAsync)
        .Produces<PrescriptionCommandResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization(Permissions.AddPrescription);

    encounterActionsGroup.MapDelete("/prescriptions", RemovePrescriptionAsync)
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization(Permissions.RemovePrescription);

    encounterActionsGroup.MapPost("/addendums", AddAddendumAsync)
        .Produces<AddendumCommandResponse>()
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization(Permissions.AddAddendum);

    encounterActionsGroup.MapPost("/lock", LockEncounterAsync)
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization(Permissions.LockEncounter);

    encounterActionsGroup.MapPost("/finalize", FinalizeEncounterAsync)
        .Produces(StatusCodes.Status200OK)
        .Produces(StatusCodes.Status400BadRequest)
        .Produces(StatusCodes.Status409Conflict)
        .Produces(StatusCodes.Status404NotFound)
        .Produces(StatusCodes.Status500InternalServerError)
        .RequireAuthorization(Permissions.FinalizeEncounter);
}

	private async Task<IResult> StartEncounterAsync(
		[FromBody] StartEncounterRequest request,
		[FromServices] ISender sender,
		HttpContext httpContext,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = request.ToCommand();
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);
		return ControllerResponse.ParseAndReturnMessage(res, new EncounterCommandResponse(res.Value!.Id));
	}
	
	private async Task<IResult> AddAddendumAsync(
		[FromRoute] string encounterId,
		[FromBody] AddAddendumRequest request,
		[FromServices] ISender sender,
		HttpContext httpContext,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = request.ToCommand(userId, encounterId);
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);
		return ControllerResponse.ParseAndReturnMessage(res, new AddendumCommandResponse(res.Value!.Id));
	}
	
	private async Task<IResult> AddDiagnosisAsync(
		[FromRoute] string encounterId,
		[FromBody] AddDiagnosisRequest request,
		[FromServices] ISender sender,
		HttpContext httpContext,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = request.ToCommand(userId, encounterId);
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);
		return ControllerResponse.ParseAndReturnMessage(res, new DiagnosisCommandResponse(res.Value!.Id));
	}
	
	private async Task<IResult> AddNoteAsync(
		[FromRoute] string encounterId,
		[FromBody] AddNoteRequest request,
		[FromServices] ISender sender,
		HttpContext httpContext,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = request.ToCommand(userId, encounterId);
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);
		return ControllerResponse.ParseAndReturnMessage(res, new NoteCommandResponse(res.Value!.Id));
	}
	
	private async Task<IResult> PrescribeMedicationAsync(
		[FromRoute] string encounterId,
		[FromBody] PrescribeMedicationRequest request,
		[FromServices] ISender sender,
		HttpContext httpContext,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = request.ToCommand(userId, encounterId);
		var res = await sender.Send(command, cancellationToken);
		if (res.IsFailure)
			return ControllerResponse.ParseAndReturnMessage(res);
		return ControllerResponse.ParseAndReturnMessage(res, new PrescriptionCommandResponse(res.Value!.Id));
	}
	
	private async Task<IResult> RemoveDiagnosisAsync(
		[FromRoute] string encounterId,
		[FromBody] RemoveDiagnosisRequest request,
		[FromServices] ISender sender,
		HttpContext httpContext,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = request.ToCommand(userId, encounterId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
	
	private async Task<IResult> RemoveNoteAsync(
		[FromRoute] string encounterId,
		[FromBody] RemoveNoteRequest request,
		[FromServices] ISender sender,
		HttpContext httpContext,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = request.ToCommand(userId, encounterId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
	
	private async Task<IResult> RemovePrescriptionAsync(
		[FromRoute] string encounterId,
		[FromBody] RemovePrescriptionRequest request,
		[FromServices] ISender sender,
		HttpContext httpContext,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = request.ToCommand(userId, encounterId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
	
	private async Task<IResult> LockEncounterAsync(
		[FromRoute] string encounterId,
		[FromServices] ISender sender,
		HttpContext httpContext,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = new LockEncounterCommand(userId, encounterId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
	}
	
	private async Task<IResult> FinalizeEncounterAsync(
		[FromRoute] string encounterId,
		[FromServices] ISender sender,
		HttpContext httpContext,
		CancellationToken cancellationToken)
	{
		var userId = httpContext.User.GetUserId();
		var command = new FinalizeEncounterCommand(userId, encounterId);
		var res = await sender.Send(command, cancellationToken);
		return ControllerResponse.ParseAndReturnMessage(res);
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
		return ControllerResponse.ParseAndReturnMessage(res, new AllergyCommandResponse(res.Value!.Id));
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