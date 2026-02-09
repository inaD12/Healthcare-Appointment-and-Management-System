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

    public IReadOnlyCollection<ClinicalNote> Notes => _notes;
    public IReadOnlyCollection<Diagnosis> Diagnoses => _diagnoses;
    public IReadOnlyCollection<Prescription> Prescriptions => _prescriptions;
    public IReadOnlyCollection<AddendumNote> Addendums => _addendums;

    public static Encounter Start(
        string patientId,
        string doctorId,
        string appointmentId,
        DateTime utcNow)
        => new(patientId, doctorId, appointmentId, utcNow);
    
    public Result AddNote(string text, DateTime utcNow)
    {
        if (Status != EncounterStatus.InProgress)
            return Result.Failure(ResponseList.EncounterNotEditable);
        
        _notes.Add(new ClinicalNote(text, utcNow));
        return Result.Success();
    }

    public Result AddDiagnosis(string icdCode, string description)
    {
        if (Status != EncounterStatus.InProgress)
            return Result.Failure(ResponseList.EncounterNotEditable);

        if (_diagnoses.Any(d => d.IcdCode == icdCode))
            return Result.Failure(ResponseList.DiagnosisAlreadyAdded);

        _diagnoses.Add(new Diagnosis(icdCode, description));
        return Result.Success();
    }

    public Result PrescribeMedication(string name, string dosage, string instructions)
    {
        if (Status != EncounterStatus.InProgress)
            return Result.Failure(ResponseList.EncounterNotEditable);
        
        _prescriptions.Add(new Prescription(name, dosage, instructions));
        return Result.Success();
    }

    public Result Finalize(DateTime utcNow)
    {
        if (!_diagnoses.Any())
            return Result.Failure(ResponseList.EncounterNeedsDiagnosis);

        Status = EncounterStatus.Finalized;
        FinalizedAt = utcNow;
        return Result.Success();
    }

    public Result Lock(DateTime utcNow)
    {
        if (Status != EncounterStatus.Finalized)
            return Result.Failure(ResponseList.LockUnfinalizedEncounter);

        Status = EncounterStatus.Locked;
        LockedAt = utcNow;
        return Result.Success();
    }

    public Result AddAddendum(string text, DateTime utcNow)
    {
        if (Status == EncounterStatus.Locked)
            return Result.Failure(ResponseList.EncounterLocked);

        if (Status == EncounterStatus.InProgress)
            return Result.Failure(ResponseList.UseNormalNotesBeforeFinalization);

        _addendums.Add(new AddendumNote(text, utcNow));
        return Result.Success();
    }
}