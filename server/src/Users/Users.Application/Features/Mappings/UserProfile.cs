using AutoMapper;
using Shared.Application.IntegrationEvents;
using Users.Domain.Events;

namespace Users.Application.Features.Mappings;

public class UserProfile : Profile
{
	public UserProfile()
	{
		CreateMap<UserCreatedDomainEvent, UserCreatedIntegrationEvent>();
	}
}