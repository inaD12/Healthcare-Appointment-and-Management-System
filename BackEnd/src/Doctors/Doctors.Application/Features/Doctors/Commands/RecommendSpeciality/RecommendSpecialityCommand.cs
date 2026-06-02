using Doctors.Application.Features.Doctors.Models;
using Shared.Application.Abstractions.Messaging;

namespace Doctors.Application.Features.Doctors.Commands.RecommendSpeciality;

public sealed record RecommendSpecialityCommand(
    string Symptoms) : ICommand<List<SpecialityViewModel>>;
