using AutoMapper;
using Users.Application.Features.Auth.Models;
using Users.Application.Features.Email.Commands.HandleEmail;
using Users.Application.Features.Users.Commands.RegisterUser;
using Users.Application.Features.Users.LoginUser;
using Users.Application.Features.Users.Models;
using Users.Application.Features.Users.UpdateUser;
using Users.Users.Models.Requests;
using Users.Users.Models.Responses;

namespace Users.Users.Mappings;

public class UserCommandMappings : Profile
{
	public UserCommandMappings()
	{
		CreateMap<LoginUserRequest, LoginUserCommand>();

		CreateMap<RegisterUserRequest, RegisterUserCommand>();

		CreateMap<(UpdateCurrentUserRequest,string id), UpdateUserCommand>()
			.ConstructUsing(src => new(
				src.Item2,
				src.Item1.NewEmail,
				src.Item1.FirstName,
				src.Item1.LastName));

		CreateMap<(UpdateUserRequest, string id), UpdateUserCommand>()
			.ConstructUsing(src => new(
				src.Item2,
				src.Item1.NewEmail,
				src.Item1.FirstName,
				src.Item1.LastName));

		CreateMap<VerifyEmailRequest, HandleEmailCommand>();

		CreateMap<LoginUserCommandViewModel, LoginUserResponse>();

		CreateMap<UserCommandViewModel, UserCommandResponse>();
	}
}
