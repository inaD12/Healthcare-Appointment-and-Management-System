using AutoMapper;
using Shared.Domain.Entities;
using Shared.Domain.Enums;
using Shared.Domain.Extensions;
using Shared.Domain.Models;
using Users.Application.Features.Users.Models;
using Users.Application.Features.Users.Queries.GetAllUsers;
using Users.Domain.Entities;
using Users.Domain.Infrastructure.Models;

namespace Users.Application.Features.Users.Mappings;

public class UserQueryProfile : Profile
{
	public UserQueryProfile()
	{
		CreateMap<Roles, Role>().ConvertUsing(role => role.MapToRole());
		CreateMap<Role, Roles>().ConvertUsing(role => role.MapToRoleEnum());
		
		CreateMap<User, UserQueryViewModel>();

		CreateMap<PagedList<User>, UserPaginatedQueryViewModel>();

		CreateMap<GetAllUsersQuery, UserPagedListQuery>();
	}
}