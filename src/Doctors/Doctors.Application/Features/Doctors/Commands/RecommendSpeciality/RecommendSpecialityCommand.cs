using Doctors.Application.Features.Doctors.Models;
using Shared.Domain.Abstractions.Messaging;

namespace Doctors.Application.Features.Doctors.Commands.RecommendSpeciality;

public sealed record RecommendSpecialityCommand(
    string Symptoms) : ICommand<List<SpecialityViewModel>>;
