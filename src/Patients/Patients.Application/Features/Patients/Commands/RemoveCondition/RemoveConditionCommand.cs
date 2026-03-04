using Shared.Domain.Abstractions.Messaging;

namespace Patients.Application.Features.Patients.Commands.RemoveCondition;

public sealed record RemoveConditionCommand(
    string Id,
    string ConditionId) : ICommand;