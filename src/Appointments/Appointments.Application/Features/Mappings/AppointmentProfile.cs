using Appointments.Domain.Entities;
using AutoMapper;
using Shared.Application.IntegrationEvents;

namespace Appointments.Application.Features.Mappings;

public class AppointmentProfile : Profile
{
	public AppointmentProfile()
	{
		CreateMap<UserCreatedIntegrationEvent, UserData>()
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
	}
}
