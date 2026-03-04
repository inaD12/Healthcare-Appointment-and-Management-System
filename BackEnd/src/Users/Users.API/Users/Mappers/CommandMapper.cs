using Shared.Domain.Extensions;
using Users.Application.Features.Email.Commands.HandleEmail;
using Users.Application.Features.Users.Commands.RegisterUser;
using Users.Application.Features.Users.Commands.UpdateUser;
using Users.Application.Features.Users.Models;
using Users.Users.Models.Requests;
using Users.Users.Models.Responses;

namespace Users.Users.Mappers;

public static class CommandMapper
{
    public static RegisterUserCommand ToCommand(
        this RegisterUserRequest request)
        => new(
            request.Email,
            request.Password,
            request.FirstName,
            request.LastName,
            request.DateOfBirth,
            request.PhoneNumber,
            request.Address,
            request.Role.MapToRole());
    
    public static UpdateUserCommand ToCommand(
        this UpdateCurrentUserRequest request,
        string userId)
        => new(
            userId,
            request.NewEmail,
            request.FirstName,
            request.LastName);
    
    public static UpdateUserCommand ToCommand(
        this UpdateUserRequest request,
        string userId)
        => new(
            userId,
            request.NewEmail,
            request.FirstName,
            request.LastName);
    
    public static HandleEmailCommand ToCommand(
        this VerifyEmailRequest request)
        => new(
            request.Token);
    
    public static LoginUserResponse ToResponse(
        this LoginUserCommandViewModel viewModel)
        => new(
            viewModel.Token);
    
    public static UserCommandResponse ToResponse(
        this UserCommandViewModel viewModel)
        => new(
            viewModel.Id);
}
  