using MediatR;
using Shared.Domain.Abstractions;
using Shared.Domain.Results;
using Users.Application.Features.Users.Queries.GetById;
using Users.Domain.Infrastructure.Models;

namespace Users.Application.Features.Users.Services;

public class NamesService(ISender sender): INamesService
{
    public async Task<Result<NamesResponse>> GetUserNamesAsync(string userId)
    {
        var query = new GetUserByIdQuery(userId);
        
        var result = await sender.Send(query);
        if (result.IsFailure)
        {
            return Result<NamesResponse>.Failure(result.Response);
        }
        
        var response = new NamesResponse(result.Value!.FirstName, result.Value.LastName);
        return Result<NamesResponse>.Success(response);
    }
}