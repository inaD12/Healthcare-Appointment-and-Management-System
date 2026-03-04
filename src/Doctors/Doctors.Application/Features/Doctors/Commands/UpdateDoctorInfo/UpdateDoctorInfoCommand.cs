using Shared.Domain.Abstractions.Messaging;

namespace Doctors.Application.Features.Doctors.Commands.UpdateDoctorInfo;

public sealed record UpdateDoctorInfoCommand(
    string UserId,
    string? NewBio = null,
    string? NewTimeZoneId = null) : ICommand;
