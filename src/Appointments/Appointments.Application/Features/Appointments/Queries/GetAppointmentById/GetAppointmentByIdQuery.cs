using Appointments.Application.Features.Appointments.Models;
using Shared.Domain.Abstractions.Messaging;

namespace Appointments.Application.Features.Appointments.Queries.GetAllAppointments;

public sealed record GetAppointmentByIdQuery(string Id) : IQuery<AppointmentQueryViewModel>;

