namespace Patients.Domain.Utilities.Strings;

public static class ErrorMessages
{
	public const string EncounterNotEditable = "Encounter is not editable";
	public const string DiagnosisAlreadyAdded = "Diagnosis already added";
	public const string EncounterNeedsDiagnosis = "Encounter requires at least one diagnosis";
	public const string LockUnfinalizedEncounter = "Only finalized encounters can be locked";
	public const string EncounterLocked = "Encounter is locked";
	public const string UseNormalNotesBeforeFinalization = "Use normal notes before finalization";
	public const string AllergyAlreadyAdded = "Allergy is already recorded";
	public const string ConditionAlreadyAdded = "Condition is already recorded";
	public const string UserIdAlreadyInUse = "UserId is already in use";
	public const string PatientNotFound = "Patient not found";
	public const string EncounterAlreadyExists = "An encounter for this appointment already exists";
	public const string EncounterNotFound = "Encounter not found";
	public const string NoteNotFound = "Note not found";
	public const string PrescriptionNotFound = "Prescription not found";
	public const string DiagnosisNotFound = "Diagnosis not found";
	public const string AllergyNotFound = "Allergy not found";
	public const string ConditionNotFound = "Condition not found";
	public const string AppointmentNotFound = "Appointment not found";
	public const string NotTheDoctor = "Only the doctor that created the encounter is allowed to edit it";
}