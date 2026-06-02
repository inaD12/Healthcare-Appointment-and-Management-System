using Appointments.Application.Features.Appointments.Models;
using Shared.Application.Abstractions.Messaging;

namespace Appointments.Application.Features.Appointments.Queries.GetAppointmentById;

public sealed record GetAppointmentByIdQuery(string Id) : IQuery<AppointmentQueryViewModel>;

