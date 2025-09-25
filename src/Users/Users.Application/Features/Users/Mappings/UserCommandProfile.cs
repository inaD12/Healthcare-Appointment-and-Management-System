using AutoMapper;
using Shared.Application.IntegrationEvents;
using Users.Application.Features.Users.Models;
using Users.Domain.Entities;

namespace Users.Application.Features.Users.Mappings;

public class UserCommandProfile : Profile
{
	public UserCommandProfile()
	{
		CreateMap<User, UserCreatedIntegrationEvent>();

		CreateMap<User, UserCommandViewModel>();
	}
}