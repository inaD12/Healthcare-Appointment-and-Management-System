namespace Doctors.API.Doctors;

internal static class Permissions
{
    // Doctor Management (Self)
    internal const string CreateDoctor = "doctor:create";
    internal const string UpdateDoctor = "doctor:update";
    internal const string ViewDoctor = "doctor:view";

    // Doctor Management (Admin)
    internal const string CreateDoctorByAdmin = "doctor:admin:create";
    internal const string UpdateDoctorByAdmin = "doctor:admin:update";
    internal const string ViewDoctorByAdmin = "doctor:admin:view";
    internal const string ViewAllDoctors = "doctor:admin:view-all";

    // Specialities
    internal const string AddSpeciality = "doctor:speciality:add";
    internal const string RemoveSpeciality = "doctor:speciality:remove";

    // Schedule (Workdays)
    internal const string AddWorkDaySchedule = "doctor:schedule:workday:add";
    internal const string ChangeWorkDaySchedule = "doctor:schedule:workday:update";
    internal const string RemoveWorkDaySchedule = "doctor:schedule:workday:remove";

    // Availability
    internal const string AddExtraAvailability = "doctor:availability:extra:add";
    internal const string RemoveExtraAvailability = "doctor:availability:extra:remove";
    internal const string AddUnavailability = "doctor:availability:unavailable:add";
    internal const string RemoveUnavailability = "doctor:availability:unavailable:remove";
}