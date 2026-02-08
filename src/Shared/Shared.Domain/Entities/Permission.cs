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

    // Doctor Management (Self)
    public static readonly Permission CreateDoctor = new("doctor:create");
    public static readonly Permission UpdateDoctor = new("doctor:update");
    public static readonly Permission ViewDoctor = new("doctor:view");

    // Doctor Management (Admin)
    public static readonly Permission CreateDoctorByAdmin = new("doctor:admin:create");
    public static readonly Permission UpdateDoctorByAdmin = new("doctor:admin:update");
    public static readonly Permission ViewDoctorByAdmin = new("doctor:admin:view");
    public static readonly Permission ViewAllDoctors = new("doctor:admin:view-all");

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
    
    public Permission(string code)
    {
        Code = code;
    }

    public string Code { get; }
}
