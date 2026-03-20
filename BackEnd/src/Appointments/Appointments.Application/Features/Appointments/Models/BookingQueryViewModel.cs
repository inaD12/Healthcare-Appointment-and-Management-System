using Appointments.Domain.Entities.Enums;
using Shared.Domain.Entities.ValueObjects;

namespace Appointments.Application.Features.Appointments.Models;

public sealed record BookingQueryViewModel(
	DateTime Start,
	DateTime End);

