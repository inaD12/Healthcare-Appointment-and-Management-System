using Shared.Domain.Abstractions;
using Shared.Domain.Models;

namespace Patients.Application.Features.AppointmentProjections.Queries.DataLoaders;

public class UserNamesDataLoader(
    IBatchScheduler batchScheduler,
    IBatchNamesService namesService,
    DataLoaderOptions? options = null)
    : BatchDataLoader<string, NamesResponse>(batchScheduler, options ?? new DataLoaderOptions())
{
    protected override async Task<IReadOnlyDictionary<string, NamesResponse>> LoadBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        GetUsersByIdsResponse response = await namesService.GetUsersNamesByIdsAsync(keys, cancellationToken);

        var dict = response.Users
            .ToDictionary(
                u => u.Id,
                u => new NamesResponse(u.FirstName, u.LastName)
            );

        return keys.ToDictionary(
            key => key,
            key => dict.TryGetValue(key, out var value) ? value : new NamesResponse("Unknown", "User")
        );
    }
}