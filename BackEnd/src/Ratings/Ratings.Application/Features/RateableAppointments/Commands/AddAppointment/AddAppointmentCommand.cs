using Shared.Domain.Abstractions.Messaging;

namespace Ratings.Application.Features.RateableAppointments.Commands.AddAppointment;

public sealed record AddAppointmentCommand(
    string AppointmentId,
    string DoctorId,
    string PatientId) : ICommand;