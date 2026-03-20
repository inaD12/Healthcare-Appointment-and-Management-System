using Appointments.Application.Features.DoctorSchedule.Commands.AddExtraAvailability;
using Appointments.Application.Features.DoctorSchedule.Commands.AddUnavailability;
using Appointments.Application.Features.DoctorSchedule.Commands.AddWorkDaySchedule;
using Appointments.Application.Features.DoctorSchedule.Commands.ChangeWorkDaySchedule;
using Appointments.Application.Features.DoctorSchedule.Commands.RemoveExtraAvailability;
using Appointments.Application.Features.DoctorSchedule.Commands.RemoveUnavailability;
using Appointments.Application.Features.DoctorSchedule.Commands.RemoveWorkDaySchedule;
using Appointments.Domain.Entities;
using Appointments.Domain.Events;
using Shared.Application.Dtos;
using Shared.Application.IntegrationEvents;

namespace Appointments.Infrastructure.Features.Mappers;

public static class EventMapper
{
    public static AppointmentCompletedIntegrationEvent ToIntegrationEvent(
        this AppointmentCompletedDomainEvent domainEvent)
        => new(
            domainEvent.AppointmentId,
            domainEvent.DoctorId,
            domainEvent.PatientId);
    
    public static AppointmentCreatedIntegrationEvent ToIntegrationEvent(
        this AppointmentCreatedDomainEvent domainEvent)
        => new(
            domainEvent.AppointmentId,
            domainEvent.DoctorId,
            domainEvent.PatientId,
            domainEvent.Start,
            domainEvent.End);
    
    public static AppointmentCanceledIntegrationEvent ToIntegrationEvent(
        this AppointmentCanceledDomainEvent domainEvent)
        => new(
            domainEvent.AppointmentId);
    
    public static AppointmentRescheduledIntegrationEvent ToIntegrationEvent(
        this AppointmentRescheduledDomainEvent domainEvent)
        => new(
            domainEvent.AppointmentId);
    
    public static AddExtraAvailabilityCommand ToCommand(
        this DoctorAddedExtraAvailabilityIntegrationEvent intEvent)
        => new(
            intEvent.DoctorUserId,
            intEvent.Start,
            intEvent.End,
            intEvent.Reason);

    public static AddUnavailabilityCommand ToCommand(
        this DoctorAddedUnavailabilityIntegrationEvent intEvent)
        => new(
            intEvent.DoctorUserId,
            intEvent.Start,
            intEvent.End,
            intEvent.Reason);
    
    public static RemoveUnavailabilityCommand ToCommand(
        this DoctorRemovedUnavailabilityIntegrationEvent intEvent)
        => new(
            intEvent.DoctorUserId,
            intEvent.Start,
            intEvent.End);
    
    public static RemoveExtraAvailabilityCommand ToCommand(
        this DoctorRemovedExtraAvailabilityIntegrationEvent intEvent)
        => new(
            intEvent.DoctorUserId,
            intEvent.Start,
            intEvent.End);
    
    public static AddWorkDayScheduleCommand ToCommand(
        this WorkDayScheduleAddedIntegrationEvent intEvent)
        => new(
            intEvent.DoctorUserId,
            intEvent.DayOfWeek,
            intEvent.WorkTimes.Select(w => w.ToWorkTimeRange()).ToList());
    
    public static ChangeWorkDayScheduleCommand ToCommand(
        this WorkDayScheduleChangedIntegrationEvent intEvent)
        => new(
            intEvent.DoctorUserId,
            intEvent.DayOfWeek,
            intEvent.WorkTimes.Select(w => w.ToWorkTimeRange()).ToList());
    
    public static RemoveWorkDayScheduleCommand ToCommand(
        this WorkDayScheduleRemovedIntegrationEvent intEvent)
        => new(
            intEvent.DoctorUserId,
            intEvent.DayOfWeek);
    
    private static WorkTimeRange ToWorkTimeRange(
        this TimeRangeDto timeRangeDto)
        => WorkTimeRange.Create(
            timeRangeDto.Start,
            timeRangeDto.End);
}