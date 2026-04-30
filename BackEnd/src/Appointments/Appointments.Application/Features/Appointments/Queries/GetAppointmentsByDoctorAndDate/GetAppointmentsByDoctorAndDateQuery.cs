using Appointments.Application.Features.Appointments.Models;
using Shared.Domain.Abstractions.Messaging;

namespace Appointments.Application.Features.Appointments.Queries.GetAppointmentsByDoctorAndDate;

public sealed record GetAppointmentsByDoctorAndDateQuery(string DoctorUserId, DateOnly StartDate, DateOnly EndDate) : IQuery<ICollection<AppointmentQueryViewModel>>;

