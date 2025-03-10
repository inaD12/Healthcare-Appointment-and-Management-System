using AutoMapper;
using Users.Application.Features.Users.Models;
using Users.Application.Features.Users.Queries.GetAllUsers;
using Users.Users.Models.Requests;
using Users.Users.Models.Responses;

namespace Users.Users.Mappings;

public class UserQueryMappings : Profile
{
	public UserQueryMappings()
	{
		CreateMap<GetAllUsersRequest, GetAllUsersQuery>();

		CreateMap<UserPaginatedQueryViewModel, UserPaginatedQueryResponse>();
	}
}
