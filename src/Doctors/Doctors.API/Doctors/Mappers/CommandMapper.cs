using Doctors.API.Doctors.Models.Requests;
using Doctors.API.Doctors.Models.Responses;
using Doctors.Application.Features.Doctors.Commands.AddExtraAvailability;
using Doctors.Application.Features.Doctors.Commands.AddSpeciality;
using Doctors.Application.Features.Doctors.Commands.AddUnavailability;
using Doctors.Application.Features.Doctors.Commands.AddWorkDaySchedule;
using Doctors.Application.Features.Doctors.Commands.ChangeWorkDaySchedule;
using Doctors.Application.Features.Doctors.Commands.CreateDoctor;
using Doctors.Application.Features.Doctors.Commands.RemoveExtraAvailability;
using Doctors.Application.Features.Doctors.Commands.RemoveUnavailability;
using Doctors.Application.Features.Doctors.Commands.RemoveWorkDaySchedule;
using Doctors.Application.Features.Doctors.Dtos;

namespace Doctors.API.Doctors.Mappers;

public static class CommandMapper
{
    public static CreateDoctorCommand ToCommand(
        this CreateDoctorRequest request,
        string userId)
        => new(
            userId,
            request.Specialities,
            request.TimeZoneId);
    
    public static AddWorkDayScheduleCommand ToCommand(
        this AddWorkDayScheduleRequest request,
        string userId)
        => new(
            userId,
            request.DayOfWeek,
            request.WorkTimes.Select(wt => wt.ToDto()).ToList());
    
    public static ChangeWorkDayScheduleCommand ToCommand(
        this ChangeWorkDayScheduleRequest request,
        string userId)
        => new(
            userId,
            request.DayOfWeek,
            request.WorkTimes.Select(wt => wt.ToDto()).ToList());
    
    public static RemoveWorkDayScheduleCommand ToCommand(
        this RemoveWorkDayScheduleRequest request,
        string userId)
        => new(
            userId,
            request.DayOfWeek);

    public static AddExtraAvailabilityCommand ToCommand(
        this AddExtraAvailabilityRequest request,
        string userId)
        => new(
            userId,
            request.Start,
            request.End,
            request.Reason);
    
    public static AddUnavailabilityCommand ToCommand(
        this AddUnavailabilityRequest request,
        string userId)
        => new(
            userId,
            request.Start,
            request.End,
            request.Reason);

    public static WorkTimeRangeDto ToDto(
        this WorkTimeRangeModel model)
        => new(
            model.Start,
            model.End);
    
    public static RemoveUnavailabilityCommand ToCommand(
        this RemoveUnavailabilityRequest request,
        string userId)
        => new(
            userId,
            request.Start,
            request.End);
    
    public static RemoveExtraAvailabilityCommand ToCommand(
        this RemoveExtraAvailabilityRequest request,
        string userId)
        => new(
            userId,
            request.Start,
            request.End);
    
    public static AddSpecialityCommand ToCommand(
        this AddSpecialityRequest request,
        string userId)
        => new(
            userId,
            request.Speciality);
}