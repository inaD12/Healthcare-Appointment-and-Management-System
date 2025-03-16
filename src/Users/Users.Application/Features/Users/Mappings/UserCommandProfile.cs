using AutoMapper;
using Shared.Application.IntegrationEvents;
using Users.Application.Features.Auth.Models;
using Users.Application.Features.Users.Models;
using Users.Domain.Entities;
using Users.Domain.Events;

namespace Users.Application.Features.Users.Mappings;

public class UserCommandProfile : Profile
{
	public UserCommandProfile()
	{
		CreateMap<User, UserCreatedIntegrationEvent>();

		CreateMap<TokenResult, LoginUserCommandViewModel>();

		CreateMap<User, UserCommandViewModel>();
	}
}