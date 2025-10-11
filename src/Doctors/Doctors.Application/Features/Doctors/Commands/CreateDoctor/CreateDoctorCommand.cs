using Doctors.Application.Features.Doctors.Models;
using Shared.Domain.Abstractions.Messaging;

namespace Doctors.Application.Features.Doctors.Commands.CreateDoctor;

public sealed record CreateDoctorCommand(
    string UserId,
    List<string> Specialities,
    string TimeZoneId ) : ICommand<DoctorCommandViewModel>;
