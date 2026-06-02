using Shared.Application.Abstractions.Messaging;

namespace Doctors.Application.Features.Doctors.Commands.UpdateDoctorInfo;

public sealed record UpdateDoctorInfoCommand(
    string UserId,
    string? NewBio = null) : ICommand;
