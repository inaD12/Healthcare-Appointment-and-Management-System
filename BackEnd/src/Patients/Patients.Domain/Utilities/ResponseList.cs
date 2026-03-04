using System.Net;
using Patients.Domain.Utilities.Strings;
using Shared.Domain.Results;

namespace Patients.Domain.Utilities;

public static class ResponseList
{
	// Success Responses

	// Error Responses
	public static Response EncounterNotEditable => Response.Create(ErrorMessages.EncounterNotEditable, HttpStatusCode.Forbidden);
	public static Response DiagnosisAlreadyAdded => Response.Create(ErrorMessages.DiagnosisAlreadyAdded, HttpStatusCode.Conflict);
	public static Response EncounterNeedsDiagnosis => Response.Create(ErrorMessages.EncounterNeedsDiagnosis, HttpStatusCode.Conflict);
	public static Response LockUnfinalizedEncounter => Response.Create(ErrorMessages.LockUnfinalizedEncounter, HttpStatusCode.Forbidden);
	public static Response EncounterLocked => Response.Create(ErrorMessages.EncounterLocked, HttpStatusCode.Conflict);
	public static Response UseNormalNotesBeforeFinalization => Response.Create(ErrorMessages.UseNormalNotesBeforeFinalization, HttpStatusCode.Forbidden);
	public static Response ConditionAlreadyAdded => Response.Create(ErrorMessages.ConditionAlreadyAdded, HttpStatusCode.Conflict);
	public static Response AllergyAlreadyAdded => Response.Create(ErrorMessages.AllergyAlreadyAdded, HttpStatusCode.Conflict);
	public static Response UserIdAlreadyInUse => Response.Create(ErrorMessages.UserIdAlreadyInUse, HttpStatusCode.Conflict);
	public static Response PatientNotFound => Response.Create(ErrorMessages.PatientNotFound, HttpStatusCode.NotFound);
	public static Response EncounterAlreadyExists => Response.Create(ErrorMessages.EncounterAlreadyExists, HttpStatusCode.Forbidden);
	public static Response EncounterNotFound => Response.Create(ErrorMessages.EncounterNotFound, HttpStatusCode.NotFound);
	public static Response NoteNotFound => Response.Create(ErrorMessages.NoteNotFound, HttpStatusCode.NotFound);
	public static Response PrescriptionNotFound => Response.Create(ErrorMessages.PrescriptionNotFound, HttpStatusCode.NotFound);
	public static Response DiagnosisNotFound => Response.Create(ErrorMessages.DiagnosisNotFound, HttpStatusCode.NotFound);
	public static Response AllergyNotFound => Response.Create(ErrorMessages.AllergyNotFound, HttpStatusCode.NotFound);
	public static Response ConditionNotFound => Response.Create(ErrorMessages.ConditionNotFound, HttpStatusCode.NotFound);
	public static Response AppointmentNotFound => Response.Create(ErrorMessages.AppointmentNotFound, HttpStatusCode.NotFound);
	public static Response NotTheDoctor => Response.Create(ErrorMessages.NotTheDoctor, HttpStatusCode.Forbidden);
}
