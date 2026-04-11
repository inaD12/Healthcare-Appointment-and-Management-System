using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Dtos;

namespace Patients.API.Patients.GraphQL.Queries.DataLoaders;

public sealed class DiagnosesByEncounterDataLoader(
    IBatchScheduler batchScheduler,
    IEncounterRepository repo,
    DataLoaderOptions? options = null)
    : GroupedDataLoader<string, DiagnosisDto>(batchScheduler, options ?? new DataLoaderOptions())
{
    protected override async Task<ILookup<string, DiagnosisDto>> LoadGroupedBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        var rows = await repo.GetDiagnosesByEncounterIdsFlatAsync(keys, cancellationToken);

        return rows.ToLookup(x => x.EncounterId);
    }
}