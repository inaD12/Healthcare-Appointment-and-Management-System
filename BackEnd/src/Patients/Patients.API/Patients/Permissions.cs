namespace Patients.API.Patients;

internal static class Permissions
{
    // Admin
    internal const string DeletePatient = "patient:delete";
    internal const string AddAllergyAdmin = "patient:admin:allergy:add";
    internal const string RemoveAllergyAdmin = "patient:admin:allergy:remove";
    internal const string AddChronicConditionAdmin = "patient:admin:condition:add";
    internal const string RemoveChronicConditionAdmin = "patient:admin:condition:remove";
    
    // Doctor
    internal const string AddAllergy = "patient:allergy:add";
    internal const string RemoveAllergy = "patient:allergy:remove";
    internal const string AddChronicCondition = "patient:condition:add";
    internal const string RemoveChronicCondition = "patient:condition:remove";

    // Encounters
    internal const string StartEncounter = "encounter:start";
    internal const string LockEncounter = "encounter:lock";
    internal const string FinalizeEncounter = "encounter:finalize";

    // Notes
    internal const string AddNote = "encounter:note:add";
    internal const string RemoveNote = "encounter:note:remove";

    // Diagnoses
    internal const string AddDiagnosis = "encounter:diagnosis:add";
    internal const string RemoveDiagnosis = "encounter:diagnosis:remove";

     // Prescriptions
    internal const string AddPrescription = "encounter:prescription:add";
    internal const string RemovePrescription = "encounter:prescription:remove";

    // Addendums
    internal const string AddAddendum = "encounter:addendum:add";
}