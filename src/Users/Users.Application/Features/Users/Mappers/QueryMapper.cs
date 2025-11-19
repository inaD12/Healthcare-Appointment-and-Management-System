using Shared.Domain.Extensions;
using Shared.Domain.Models;
using Users.Application.Features.Users.Models;
using Users.Application.Features.Users.Queries.GetAllUsers;
using Users.Domain.Entities;
using Users.Domain.Infrastructure.Models;

namespace Users.Application.Features.Users.Mappers;

public static class QueryMapper
{
    public static UserQueryViewModel ToQueryViewModel(
        this User user)
        => new(
            user.Id,
            user.Email,
            user.Roles.Select(role => role.MapToRoleEnum()).ToList(),
            user.FirstName,
            user.LastName,
            user.PhoneNumber,
            user.Address,
            user.EmailVerified);
    
    public static UserPaginatedQueryViewModel ToQueryViewModel(
        this PagedList<User> list)
        => new(
            list.Items.Select(u => u.ToQueryViewModel()).ToList(),
            list.Page,
            list.PageSize,
            list.TotalCount,
            list.HasNextPage,
            list.HasPreviousPage);
    
    public static UserPagedListQuery ToPagedListQuery(
        this GetAllUsersQuery query)
        => new(
            query.Email,
            query.Role,
            query.FirstName,
            query.LastName,
            query.PhoneNumber,
            query.Address,
            query.EmailVerified,
            query.SortOrder,
            query.SortPropertyName,
            query.Page,
            query.PageSize
            );
}