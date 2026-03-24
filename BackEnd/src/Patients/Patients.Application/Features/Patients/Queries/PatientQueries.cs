using Microsoft.AspNetCore.Http;
using Patients.Application.Features.Patients.Dtos;
using Patients.Domain.Abstractions.Repositories;
using Shared.Infrastructure.Authentication;

namespace Patients.Application.Features.Patients.Queries;

public sealed class PatientQueries
{
    [UsePaging(IncludeTotalCount = true)]
    [UseFiltering]
    [UseSorting]
    public IQueryable<PatientListItemDto> GetPatients(
        [Service] IPatientRepository repo)
        => repo.GetAll();

    public async Task<PatientHeaderDto> GetMyPatientHeader(
        HttpContext httpContext,
        [Service] IPatientRepository repo)
    {
        string userId = httpContext.User.GetUserId();

        var header = await repo.GetHeaderAsync(userId);

        return header;
    }
}