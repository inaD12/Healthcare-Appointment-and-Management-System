using Appointments.Domain.Entities;
using AutoMapper;
using Shared.Application.IntegrationEvents;
using Shared.Domain.Extensions;

namespace Appointments.Application.Features.Mappings;

public class AppointmentProfile : Profile
{
	public AppointmentProfile()
	{
		CreateMap<UserCreatedIntegrationEvent, UserData>()
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
				.ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles.Select(p => p.MapToRole()).ToList()));
	}
}
