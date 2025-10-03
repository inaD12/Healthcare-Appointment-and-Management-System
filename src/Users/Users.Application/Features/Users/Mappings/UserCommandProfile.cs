using AutoMapper;
using Shared.Application.IntegrationEvents;
using Shared.Domain.Entities;
using Shared.Domain.Enums;
using Shared.Domain.Extensions;
using Users.Application.Features.Users.Models;
using Users.Domain.Entities;

namespace Users.Application.Features.Users.Mappings;

public class UserCommandProfile : Profile
{
	public UserCommandProfile()
	{
		CreateMap<Roles, Role>().ConvertUsing(role => role.MapToRole());
		CreateMap<Role, Roles>().ConvertUsing(role => role.MapToRoleEnum());
		
		CreateMap<User, UserCreatedIntegrationEvent>();

		CreateMap<User, UserCommandViewModel>();
	}
}