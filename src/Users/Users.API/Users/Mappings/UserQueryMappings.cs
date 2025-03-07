using AutoMapper;
using Shared.Domain.Enums;
using Users.Application.Features.Users.Models;
using Users.Application.Features.Users.Queries.GetAllUsers;
using Users.Users.Models.Requests;
using Users.Users.Models.Responses;

namespace Users.Users.Mappings;

public class UserQueryMappings : Profile
{
	public UserQueryMappings()
	{
		CreateMap<GetAllUsersRequest, GetAllUsersQuery>()
			.BeforeMap((src, dest) =>
			{
				src.SortPropertyName ??= "Id";
				src.Page ??= 1;
				src.PageSize ??= 10;
				src.SortOrder ??= SortOrder.ASC;
			});

		CreateMap<UserPaginatedQueryViewModel, UserPaginatedQueryResponse>();
	}
}
