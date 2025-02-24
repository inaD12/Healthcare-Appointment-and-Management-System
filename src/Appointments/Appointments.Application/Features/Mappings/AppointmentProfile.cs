using Appointments.Domain.Entities;
using AutoMapper;
using Shared.Domain.Events;

namespace Appointments.Application.Features.Mappings;

public class AppointmentProfile : Profile
{
	public AppointmentProfile()
	{
		CreateMap<UserCreatedEvent, UserData>()
				.ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id));
	}
}
