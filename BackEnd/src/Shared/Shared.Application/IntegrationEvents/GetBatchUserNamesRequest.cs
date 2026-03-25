namespace Shared.Application.IntegrationEvents;

public sealed record GeBatchUserNamesRequest(IEnumerable<string> UserIds);