using AutoMapper;
using Shared.Domain.Events;
using Users.Application.Features.Auth.Models;
using Users.Application.Features.Email.Models;
using Users.Application.Features.Users.Commands.RegisterUser;
using Users.Application.Features.Users.Models;
using Users.Domain.Entities;

namespace Users.Application.Features.Users.Mappings;

public class UserCommandProfile : Profile
{
	public UserCommandProfile()
	{
		CreateMap<User, UserCreatedEvent>();

		CreateMap<User, PublishEmailConfirmationTokenModel>()
			.ConstructUsing(src => new(
				src.Email,
				src.Id));

		CreateMap<TokenResult, LoginUserCommandViewModel>();

		CreateMap<User, UserCommandViewModel>();
	}
}