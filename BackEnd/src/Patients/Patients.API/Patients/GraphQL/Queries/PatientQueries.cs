using System.Data.Entity;
using Patients.Application.Features.Patients.Dtos;
using Patients.Domain.Abstractions.Repositories;
using Patients.Domain.Abstractions.Repositories.Command;
using Patients.Domain.Abstractions.Repositories.Query;
using Patients.Infrastructure.Features.DBContexts;
using Shared.Infrastructure.Authentication;

namespace Patients.API.Patients.GraphQL.Queries;

public sealed class PatientQueries
{
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<PatientListItemDto> GetPatients(
        PatientsQueryDbContext queryDbContext)
    {
        return queryDbContext.Patients
            .AsNoTracking()
            .Select(p => new PatientListItemDto(
                p.Id,
                p.FirstName + " " + p.LastName,
                p.BirthDate
            ));
    }

    public async Task<PatientHeaderDto> GetMyPatientHeader(
        HttpContext httpContext,
        [Service] IPatientQueryRepository repo)
    {
        string userId = httpContext.User.GetUserId();

        var header = await repo.GetHeaderAsync(userId);

        return header;
    }
    
    public async Task<PatientHeaderDto> GetPatientHeaderByUserId(
        string userId,
        [Service] IPatientQueryRepository repo)
    {
        var header = await repo.GetHeaderAsync(userId);

        return header;
    }
}