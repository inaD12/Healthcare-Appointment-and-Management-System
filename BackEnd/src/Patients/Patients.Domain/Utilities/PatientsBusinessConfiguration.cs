namespace Patients.Domain.Utilities;

public class PatientsBusinessConfiguration
{
	public const int ID_MIN_LENGTH = 7;
	public const int ID_MAX_LENGTH = 100;
	
	public const int FIRSTNAME_MIN_LENGTH = 3;
	public const int FIRSTNAME_MAX_LENGTH = 30;

	public const int LASTTNAME_MIN_LENGTH = 3;
	public const int LASTNAME_MAX_LENGTH = 30;
	
	public const int SUBSTANCE_MIN_LENGTH = 5;
	public const int SUBSTANCE_MAX_LENGTH = 200;

	public const int REACTION_MIN_LENGTH = 5;
	public const int REACTION_MAX_LENGTH = 500;
	
	public const int CHRONIC_CONDITION_NAME_MIN_LENGTH = 5;
	public const int CHRONIC_CONDITION_NAME_MAX_LENGTH = 100;
	
	public const int CLINICAL_NOTE_TEXT_MIN_LENGTH = 5;
	public const int CLINICAL_NOTE_TEXT_MAX_LENGTH = 400;
	
	public const int ICD_MIN_LENGTH = 1;
	public const int ICD_MAX_LENGTH = 16;

	public const int DIAGNOSIS_DESCTIPTION_MIN_LENGTH = 5;
	public const int DIAGNOSIS_DESCTIPTION_MAX_LENGTH = 500;
	
	public const int PRESCRIPTION_NAME_MIN_LENGTH = 5;
	public const int PRESCRIPTION_NAME_MAX_LENGTH = 200;
	
	public const int PRESCRIPTION_DOSAGE_MIN_LENGTH = 1;
	public const int PRESCRIPTION_DOSAGE_MAX_LENGTH = 100;

	public const int PRESCRIPTION_INSTRUCTIONS_MIN_LENGTH = 5;
	public const int PRESCRIPTION_INSTRUCTIONS_MAX_LENGTH = 1000;
		
	public const int ADDENDUM_NOTE_TEXT_MIN_LENGTH = 5;
	public const int ADDENDUM_NOTE_TEXT_MAX_LENGTH = 400;
	
}
