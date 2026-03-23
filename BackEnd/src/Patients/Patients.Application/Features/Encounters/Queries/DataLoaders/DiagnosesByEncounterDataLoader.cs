using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Dtos;

namespace Patients.Application.Features.Encounters.Queries.DataLoaders;

public sealed class DiagnosesByEncounterDataLoader(
    IBatchScheduler batchScheduler,
    IEncounterRepository encounterRepository,
    DataLoaderOptions? options = null)
    : BatchDataLoader<string, List<DiagnosisDto>>(batchScheduler, options ?? new DataLoaderOptions())
{
    protected override async Task<IReadOnlyDictionary<string, List<DiagnosisDto>>> LoadBatchAsync(
        IReadOnlyList<string> keys,
        CancellationToken cancellationToken)
    {
        return await encounterRepository.GetDiagnosesByEncounterIdsAsync(keys, cancellationToken);
    }
}