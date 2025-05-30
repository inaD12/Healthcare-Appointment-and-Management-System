using AutoMapper;
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
		CreateMap<User, UserQueryViewModel>();

		CreateMap<PagedList<User>, UserPaginatedQueryViewModel>();

		CreateMap<GetAllUsersQuery, UserPagedListQuery>();
	}
}