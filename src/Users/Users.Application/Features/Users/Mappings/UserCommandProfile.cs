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
		CreateMap<(PasswordHashResult, RegisterUserCommand), User>()
			.ConstructUsing(src => new(
				src.Item2.Email,
				src.Item1.PasswordHash,
				src.Item1.Salt,
				src.Item2.Role,
				src.Item2.FirstName,
				src.Item2.LastName,
				src.Item2.DateOfBirth.ToUniversalTime(),
				src.Item2.PhoneNumber,
				src.Item2.Address,
				false));


		CreateMap<User, UserCreatedEvent>();

		CreateMap<User, PublishEmailConfirmationTokenModel>()
		.ConstructUsing(src => new(
			src.Email,
			src.Id));

		CreateMap<TokenResult, LoginUserCommandViewModel>();

		CreateMap<User, UserCommandViewModel>();
	}
}