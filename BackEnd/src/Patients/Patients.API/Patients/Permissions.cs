namespace Patients.API.Patients;

internal static class Permissions
{
    // Basic patient data
    internal const string CreatePatient = "patient:create";
    internal const string UpdatePatient = "patient:update";
    internal const string ViewPatient = "patient:view";
    internal const string DeletePatient = "patient:delete";

    // Admin
    internal const string ViewAllPatients = "patient:admin:view-all";
    internal const string DeletePatientByAdmin = "patient:admin:delete";

    internal const string AddAllergy = "patient:allergy:add";
    internal const string RemoveAllergy = "patient:allergy:remove";
    internal const string ViewAllergies = "patient:allergy:view";

    internal const string AddChronicCondition = "patient:condition:add";
    internal const string RemoveChronicCondition = "patient:condition:remove";
    internal const string ViewChronicConditions = "patient:condition:view";

    // Encounters
    internal const string StartEncounter = "encounter:start";
    internal const string ViewEncounter = "encounter:view";
    internal const string EditEncounter = "encounter:edit";
    internal const string LockEncounter = "encounter:lock";
    internal const string FinalizeEncounter = "encounter:finalize";

    // Notes
    internal const string AddNote = "encounter:note:add";
    internal const string RemoveNote = "encounter:note:remove";
    internal const string ViewNotes = "encounter:note:view";

    // Diagnoses
    internal const string AddDiagnosis = "encounter:diagnosis:add";
    internal const string RemoveDiagnosis = "encounter:diagnosis:remove";
    internal const string ViewDiagnoses = "encounter:diagnosis:view";

     // Prescriptions
    internal const string AddPrescription = "encounter:prescription:add";
    internal const string RemovePrescription = "encounter:prescription:remove";
    internal const string ViewPrescriptions = "encounter:prescription:view";

    // Addendums
    internal const string AddAddendum = "encounter:addendum:add";
    internal const string ViewAddendums = "encounter:addendum:view";
}