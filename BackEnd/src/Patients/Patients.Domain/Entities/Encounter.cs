using Patients.Domain.Enums;
using Patients.Domain.Utilities;
using Patients.Domain.ValueObjects;
using Shared.Domain.Entities.Base;
using Shared.Domain.Results;

namespace Patients.Domain.Entities;

public sealed class Encounter : BaseConcurrencyEntity
{
    private readonly List<ClinicalNote> _notes = new();
    private readonly List<Diagnosis> _diagnoses = new();
    private readonly List<Prescription> _prescriptions = new();
    private readonly List<AddendumNote> _addendums = new();

    private Encounter()
    {
    }

    private Encounter(
        string patientId,
        string doctorId,
        string appointmentId,
        DateTime utcNow)
    {
        PatientId = patientId;
        DoctorId = doctorId;
        AppointmentId = appointmentId;
        Status = EncounterStatus.InProgress;
        StartedAt = utcNow;
    }

    public string PatientId { get; private set; }
    public string DoctorId { get; private set; }
    public string AppointmentId { get; private set; }

    public EncounterStatus Status { get; private set; }
    public DateTime StartedAt { get; private set; }
    public DateTime? FinalizedAt { get; private set; }
    public DateTime? LockedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    public IReadOnlyCollection<ClinicalNote> Notes => _notes;
    public IReadOnlyCollection<Diagnosis> Diagnoses => _diagnoses;
    public IReadOnlyCollection<Prescription> Prescriptions => _prescriptions;
    public IReadOnlyCollection<AddendumNote> Addendums => _addendums;
    
    private IEnumerable<ClinicalNote> ActiveNotes =>
        _notes.Where(n => !n.IsDeleted);

    private IEnumerable<Diagnosis> ActiveDiagnoses =>
        _diagnoses.Where(d => !d.IsDeleted);

    private IEnumerable<Prescription> ActivePrescriptions =>
        _prescriptions.Where(p => !p.IsDeleted);
    
    public static Result<Encounter> Start(
        string patientId,
        string doctorId,
        string appointmentId,
        DateTime utcNow,
        AppointmentStatus status)
    {
        if (status != AppointmentStatus.Completed && status != AppointmentStatus.Scheduled)
            return Result<Encounter>.Failure(ResponseList.AppointmentNotCompletedOrScheduled);
        
        var encounter = new Encounter(patientId, doctorId, appointmentId, utcNow);
        return Result<Encounter>.Success(encounter); 
    }

    public Result<string> AddNote(string text, DateTime utcNow)
    {
        if (Status != EncounterStatus.InProgress)
            return Result<string>.Failure(ResponseList.EncounterNotEditable);

        var note = new ClinicalNote(text, utcNow);
        
        _notes.Add(note);
        UpdatedAt = utcNow;
        return Result<string>.Success(note.Id);
    }

    public Result<string> AddDiagnosis(string icdCode, string description, DateTime utcNow)
    {
        if (Status != EncounterStatus.InProgress)
            return Result<string>.Failure(ResponseList.EncounterNotEditable);

        if (ActiveDiagnoses.Any(d => d.IcdCode == icdCode))
            return Result<string>.Failure(ResponseList.DiagnosisAlreadyAdded);

        var diagnosis = new Diagnosis(icdCode, description, utcNow);
        
        _diagnoses.Add(diagnosis);
        UpdatedAt = utcNow;
        return Result<string>.Success(diagnosis.Id);
    }

    public Result<string> PrescribeMedication(string name, string dosage, string instructions, DateTime utcNow)
    {
        if (Status != EncounterStatus.InProgress)
            return Result<string>.Failure(ResponseList.EncounterNotEditable);

        var presctiption = new Prescription(name, dosage, instructions, utcNow);
        
        _prescriptions.Add(presctiption);
        UpdatedAt = utcNow;
        return Result<string>.Success(presctiption.Id);
    }

    public Result RemoveNote(string noteId, DateTime utcNow)
    {
        if (Status != EncounterStatus.InProgress)
            return Result.Failure(ResponseList.EncounterNotEditable);

        var note = ActiveNotes.FirstOrDefault(n => n.Id == noteId);
        if (note is null)
            return Result.Failure(ResponseList.NoteNotFound);
        
        note.Delete(utcNow);
        UpdatedAt = utcNow;
        return Result.Success();
    }
    
    public Result RemoveDiagnosis(string diagnosisId, DateTime utcNow)
    {
        if (Status != EncounterStatus.InProgress)
            return Result.Failure(ResponseList.EncounterNotEditable);

        var diagnoses = ActiveDiagnoses.FirstOrDefault(n => n.Id == diagnosisId);
        if (diagnoses is null)
            return Result.Failure(ResponseList.DiagnosisNotFound);

        diagnoses.Delete(utcNow);
        UpdatedAt = utcNow;
        return Result.Success();
    }
    
    public Result RemovePrescription(string prescriptionId, DateTime utcNow)
    {
        if (Status != EncounterStatus.InProgress)
            return Result.Failure(ResponseList.EncounterNotEditable);

        var prescription = ActivePrescriptions.FirstOrDefault(n => n.Id == prescriptionId);
        if (prescription is null)
            return Result.Failure(ResponseList.PrescriptionNotFound);

        prescription.Delete(utcNow);
        UpdatedAt = utcNow;
        return Result.Success();
    }

    public Result Finalize(DateTime utcNow)
    {
        if (!_diagnoses.Any())
            return Result.Failure(ResponseList.EncounterNeedsDiagnosis);

        Status = EncounterStatus.Finalized;
        FinalizedAt = utcNow;
        UpdatedAt = utcNow;
        return Result.Success();
    }

    public Result Lock(DateTime utcNow)
    {
        if (Status != EncounterStatus.Finalized)
            return Result.Failure(ResponseList.LockUnfinalizedEncounter);

        Status = EncounterStatus.Locked;
        LockedAt = utcNow;
        UpdatedAt = utcNow;
        return Result.Success();
    }
    
    public Result Unlock(DateTime utcNow)
    {
        if (Status != EncounterStatus.Locked)
            return Result.Failure(ResponseList.NotLocked);

        Status = EncounterStatus.Finalized;
        LockedAt = null;
        UpdatedAt = utcNow;
        return Result.Success();
    }
    
    public Result UnFinalize(DateTime utcNow)
    {
        if (Status != EncounterStatus.Finalized)
            return Result.Failure(ResponseList.NotFinalized);

        Status = EncounterStatus.InProgress;
        FinalizedAt = null;
        UpdatedAt = utcNow;
        return Result.Success();
    }

    public Result<string> AddAddendum(string text, DateTime utcNow)
    {
        if (Status == EncounterStatus.Locked)
            return Result<string>.Failure(ResponseList.EncounterLocked);

        if (Status == EncounterStatus.InProgress)
            return Result<string>.Failure(ResponseList.UseNormalNotesBeforeFinalization);

        var addendum = new AddendumNote(text, utcNow);
        
        _addendums.Add(addendum);
        UpdatedAt = utcNow;
        return Result<string>.Success(addendum.Id);
    }
}