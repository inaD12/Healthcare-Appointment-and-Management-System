using AutoMapper;
using Users.Application.Features.Users.Models;
using Users.Application.Features.Users.Queries.GetAllUsers;
using Users.Application.Features.Users.Queries.GetById;
using Users.Extensions;
using Users.Users.Models.Requests;
using Users.Users.Models.Responses;

namespace Users.Users.Mappings;

public class UserQueryMappings : Profile
{
	public UserQueryMappings()
	{
		CreateMap<string, GetUserByIdQuery>()
			.ConstructUsing(src => new(src));

		CreateMap<GetAllUsersRequest, GetAllUsersQuery>()
			.ForMember(dest => dest.Role,
				opt => opt.MapFrom(src => src.Role.MapRole()));

		CreateMap<UserQueryViewModel, UserQueryResponse>();

		CreateMap<UserPaginatedQueryViewModel, UserPaginatedQueryResponse>();
	}
}
