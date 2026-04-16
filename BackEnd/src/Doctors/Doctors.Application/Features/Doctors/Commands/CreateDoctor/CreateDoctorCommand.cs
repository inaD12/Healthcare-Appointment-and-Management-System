using Doctors.Application.Features.Doctors.Models;
using Shared.Domain.Abstractions.Messaging;

namespace Doctors.Application.Features.Doctors.Commands.CreateDoctor;

public sealed record CreateDoctorCommand(
    string UserId,
    string FirstName,
    string LastName,
    string? Bio,
    List<string>? Specialities) : ICommand<DoctorCommandViewModel>;
