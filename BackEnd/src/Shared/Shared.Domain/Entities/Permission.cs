namespace Shared.Domain.Entities;

public sealed class Permission
{
    // Users
    public static readonly Permission GetUser = new("users:read");
    public static readonly Permission ModifyUser = new("users:update");
    public static readonly Permission DeleteUser = new("users:delete");

    // Appointments
    public static readonly Permission CreateAppointment = new("appointment:create");
    public static readonly Permission CancelAppointment = new("appointment:cancel");
    public static readonly Permission RescheduleAppointment = new("appointment:reschedule");
    public static readonly Permission GetAppointment = new("appointment:read");
    public static readonly Permission GetBookings = new("bookings:read");
    public static readonly Permission GetMyAppointment = new("appointment:mine:read");
    
    // Doctor Management (Self)
    public static readonly Permission CreateDoctor = new("doctor:create");
    public static readonly Permission UpdateDoctor = new("doctor:update");
    public static readonly Permission ViewDoctor = new("doctor:view");
    public static readonly Permission ViewAllDoctors = new("doctor:view-all");

    // Doctor Management (Admin)
    public static readonly Permission CreateDoctorByAdmin = new("doctor:admin:create");
    public static readonly Permission UpdateDoctorByAdmin = new("doctor:admin:update");
    public static readonly Permission ViewDoctorByAdmin = new("doctor:admin:view");

    // Specialities
    public static readonly Permission AddSpeciality = new("doctor:speciality:add");
    public static readonly Permission RemoveSpeciality = new("doctor:speciality:remove");
    public static readonly Permission RequestRecommendations = new("doctor:speciality:recommend");

    // Schedule (Workdays)
    public static readonly Permission AddWorkDaySchedule = new("doctor:schedule:workday:add");
    public static readonly Permission ChangeWorkDaySchedule = new("doctor:schedule:workday:update");
    public static readonly Permission RemoveWorkDaySchedule = new("doctor:schedule:workday:remove");

    // Availability
    public static readonly Permission AddExtraAvailability = new("doctor:availability:extra:add");
    public static readonly Permission RemoveExtraAvailability = new("doctor:availability:extra:remove");
    public static readonly Permission AddUnavailability = new("doctor:availability:unavailable:add");
    public static readonly Permission RemoveUnavailability = new("doctor:availability:unavailable:remove");
    
    // Ratings
    public static readonly Permission AddRating = new("ratings:create");
    public static readonly Permission EditRating = new("ratings:update");
    public static readonly Permission RemoveRating = new("ratings:delete");
    public static readonly Permission GetRating = new("ratings:read");
    public static readonly Permission GetRatingStats = new("ratingStats:read");
    
    // Patients
    public static readonly Permission CreatePatient = new("patient:create");
    public static readonly Permission UpdatePatient = new("patient:update");
    public static readonly Permission ViewPatient = new("patient:view");
    public static readonly Permission DeletePatient = new("patient:delete");

    // Admin patient
    public static readonly Permission ViewAllPatients = new("patient:admin:view-all");
    public static readonly Permission DeletePatientByAdmin = new("patient:admin:delete");

    // Allergies
    public static readonly Permission AddAllergy = new("patient:allergy:add");
    public static readonly Permission RemoveAllergy = new("patient:allergy:remove");
    public static readonly Permission ViewAllergies = new("patient:allergy:view");

    // Chronic conditions
    public static readonly Permission AddChronicCondition = new("patient:condition:add");
    public static readonly Permission RemoveChronicCondition = new("patient:condition:remove");
    public static readonly Permission ViewChronicConditions = new("patient:condition:view");

    // Encounter Permissions
    public static readonly Permission StartEncounter = new("encounter:start");
    public static readonly Permission ViewEncounter = new("encounter:view");
    public static readonly Permission EditEncounter = new("encounter:edit");
    public static readonly Permission LockEncounter = new("encounter:lock");
    public static readonly Permission FinalizeEncounter = new("encounter:finalize");

    // Notes
    public static readonly Permission AddNote = new("encounter:note:add");
    public static readonly Permission RemoveNote = new("encounter:note:remove");
    public static readonly Permission ViewNotes = new("encounter:note:view");

    // Diagnoses
    public static readonly Permission AddDiagnosis = new("encounter:diagnosis:add");
    public static readonly Permission RemoveDiagnosis = new("encounter:diagnosis:remove");
    public static readonly Permission ViewDiagnoses = new("encounter:diagnosis:view");

    // Prescriptions
    public static readonly Permission AddPrescription = new("encounter:prescription:add");
    public static readonly Permission RemovePrescription = new("encounter:prescription:remove");
    public static readonly Permission ViewPrescriptions = new("encounter:prescription:view");

    // Addendums
    public static readonly Permission AddAddendum = new("encounter:addendum:add");
    public static readonly Permission ViewAddendums = new("encounter:addendum:view");
    
    public Permission(string code)
    {
        Code = code;
    }

    public string Code { get; }
}
