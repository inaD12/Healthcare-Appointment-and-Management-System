using Doctors.Application.Features.Specialities.Models;
using Doctors.Application.Features.Specialities.Queries.GetAllDoctors;
using Doctors.Domain.Entities;
using Shared.Domain.Models;

namespace Doctors.Application.Features.Specialities.Mappers;

public static class QueryMapper
{
    public static SpecialityQueryViewModel ToQueryViewModel(
        this Speciality doctor)
        => new(
            doctor.Id,
            doctor.Name,
            doctor.Description);

    public static SpecialityPaginatedQueryViewModel ToViewModel(
        this PagedList<Speciality> pagedList)
        => new(
            pagedList.Items.Select(i => i.ToQueryViewModel()).ToList(),
            pagedList.Page,
            pagedList.PageSize,
            pagedList.TotalCount,
            pagedList.HasNextPage,
            pagedList.HasPreviousPage);

    public static SpecialityPagedListQuery ToInfraQuery(
        this GetAllSpecialitiesQuery query)
        => new(
            query.Name,
            query.Description,
            query.SortOrder,
            query.SortPropertyName,
            query.Page,
            query.PageSize);
}