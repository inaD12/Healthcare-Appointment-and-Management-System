using Shared.Domain.Abstractions.Messaging;

namespace Patients.Application.Features.Patients.Commands.AddChronicCondition;

public sealed record AddChronicConditionCommand(
    string Id,
    string Name) : ICommand;