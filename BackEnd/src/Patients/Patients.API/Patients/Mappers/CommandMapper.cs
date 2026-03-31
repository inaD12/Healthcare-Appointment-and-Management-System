using Patients.API.Patients.Models.Requests;
using Patients.Application.Features.Encounters.Commands.AddAddendum;
using Patients.Application.Features.Encounters.Commands.AddDiagnosis;
using Patients.Application.Features.Encounters.Commands.AddNote;
using Patients.Application.Features.Encounters.Commands.LockEncounter;
using Patients.Application.Features.Encounters.Commands.PrescribeMedication;
using Patients.Application.Features.Encounters.Commands.RemoveDiagnosis;
using Patients.Application.Features.Encounters.Commands.RemoveNote;
using Patients.Application.Features.Encounters.Commands.RemovePrescription;
using Patients.Application.Features.Encounters.Commands.StartEncounter;
using Patients.Application.Features.Patients.Commands.AddAllergy;
using Patients.Application.Features.Patients.Commands.AddChronicCondition;
using Patients.Application.Features.Patients.Commands.RemoveAllergy;
using Patients.Application.Features.Patients.Commands.RemoveCondition;

namespace Patients.API.Patients.Mappers;

public static class CommandMapper
{
    public static AddAddendumCommand ToCommand(
        this AddAddendumRequest request,
        string userId,
        string encounterId)
        => new(
            userId,
            encounterId,
            request.Note);
    
    public static AddAllergyCommand ToCommand(
        this AddAllergyRequest request,
        string patientId)
        => new(
            patientId,
            request.Substance,
            request.Reaction);
    
    public static AddChronicConditionCommand ToCommand(
        this AddChronicConditionRequest request,
        string patientId)
        => new(
            patientId,
            request.Name);
    
    public static AddDiagnosisCommand ToCommand(
        this AddDiagnosisRequest request,
        string userId,
        string encounterId)
        => new(
            userId,
            encounterId,
            request.IcdCode,
            request.Description);
    
    public static AddNoteCommand ToCommand(
        this AddNoteRequest request,
        string userId,
        string encounterId)
        => new(
            userId,
            encounterId,
            request.Note);
    
    public static PrescribeMedicationCommand ToCommand(
        this PrescribeMedicationRequest request,
        string userId,
        string encounterId)
        => new(
            userId,
            encounterId,
            request.Name,
            request.Dosage,
            request.Instructions);
    
    public static RemoveAllergyCommand ToCommand(
        this RemoveAllergyRequest request,
        string patientId)
        => new(
            patientId,
            request.AllergyId);
    
    public static RemoveConditionCommand ToCommand(
        this RemoveConditionRequest request,
        string patientId)
        => new(
            patientId,
            request.ConditionId);
    
    public static RemoveDiagnosisCommand ToCommand(
        this RemoveDiagnosisRequest request,
        string userId,
        string encounterId)
        => new(
            userId,
            encounterId,
            request.DiagnosisId);
    
    public static RemoveNoteCommand ToCommand(
        this RemoveNoteRequest request,
        string userId,
        string encounterId)
        => new(
            userId,
            encounterId,
            request.NoteId);
    
    public static RemovePrescriptionCommand ToCommand(
        this RemovePrescriptionRequest request,
        string userId,
        string encounterId)
        => new(
            userId,
            encounterId,
            request.PrescriptionId);
    
    public static StartEncounterCommand ToCommand(
        this StartEncounterRequest request)
        => new(
            request.AppointmentId);
}
  