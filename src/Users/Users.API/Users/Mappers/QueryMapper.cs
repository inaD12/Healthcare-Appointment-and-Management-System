using Shared.Domain.Extensions;
using Users.Application.Features.Users.Models;
using Users.Application.Features.Users.Queries.GetAllUsers;
using Users.Users.Models.Requests;
using Users.Users.Models.Responses;

namespace Users.Users.Mappers;

public static class QueryMapper
{
    public static GetAllUsersQuery ToQuery(
        this GetAllUsersRequest request)
        => new(
            request.Email,
            request.Role.MapToRole(),
            request.FirstName,
            request.LastName,
            request.PhoneNumber,
            request.Address,
            request.EmailVerified,
            request.SortOrder,
            request.Page,
            request.PageSize,
            request.SortPropertyName);
    
    public static UserQueryResponse ToResponse(
        this UserQueryViewModel viewModel)
        => new(
            viewModel.Id,
            viewModel.Email,
            viewModel.Roles.ToList(),
            viewModel.FirstName,
            viewModel.LastName,
            viewModel.PhoneNumber,
            viewModel.Address,
            viewModel.EmailVerified);
    
    public static UserPaginatedQueryResponse ToResponse(
        this UserPaginatedQueryViewModel viewModel)
        => new(
            viewModel.Items,
            viewModel.Page,
            viewModel.PageSize,
            viewModel.TotalCount,
            viewModel.HasNextPage,
            viewModel.HasPreviousPage);
}