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
}