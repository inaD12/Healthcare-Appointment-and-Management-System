using AutoMapper;
using Shared.Domain.Entities;
using Shared.Domain.Enums;
using Shared.Domain.Extensions;
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
		CreateMap<Roles, Role>().ConvertUsing(role => role.MapToRole());
		CreateMap<Role, Roles>().ConvertUsing(role => role.MapToRoleEnum());
		
		CreateMap<string, GetUserByIdQuery>()
			.ConstructUsing(src => new(src));

		CreateMap<GetAllUsersRequest, GetAllUsersQuery>()
			.ForMember(dest => dest.Role,
				opt => opt.MapFrom(src => src.Role.MapToRole()));

		CreateMap<UserQueryViewModel, UserQueryResponse>();

		CreateMap<UserPaginatedQueryViewModel, UserPaginatedQueryResponse>();
	}
}
