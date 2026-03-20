using Appointments.Application.Features.Appointments.Models;
using Shared.Domain.Abstractions.Messaging;

namespace Appointments.Application.Features.Appointments.Queries.GetBookingsByDoctorAndDate;

public sealed record GetBookingsByDoctorAndDateQuery(string DoctorUserId, DateOnly Date) : IQuery<ICollection<BookingQueryViewModel>>;

