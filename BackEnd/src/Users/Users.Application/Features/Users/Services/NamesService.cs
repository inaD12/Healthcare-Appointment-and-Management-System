using MediatR;
using Shared.Domain.Abstractions;
using Shared.Domain.Models;
using Shared.Domain.Results;
using Users.Application.Features.Users.Queries.GetUserById;
using Users.Domain.Abstractions.Repositories;

namespace Users.Application.Features.Users.Services;

public class NamesService(ISender sender, IUserRepository repository): INamesService, IBatchNamesService
{
    public async Task<Result<NamesResponse>> GetUserNamesAsync(string userId, CancellationToken cancellationToken = default)
    {
        var query = new GetUserByIdQuery(userId);
        
        var result = await sender.Send(query, cancellationToken);
        if (result.IsFailure)
        {
            return Result<NamesResponse>.Failure(result.Response);
        }
        
        var response = new NamesResponse(result.Value!.FirstName, result.Value.LastName);
        return Result<NamesResponse>.Success(response);
    }
    
    public async Task<GetUsersByIdsResponse> GetUsersNamesByIdsAsync(
        IEnumerable<string> userIds,
        CancellationToken cancellationToken = default)
    {
       var res = await repository.GetNamesByIdsAsync(userIds, cancellationToken);

       var response = new GetUsersByIdsResponse
       {
           Users = res.Select(u => new UserNameModel
           {
               Id = u.Key,
               FirstName = u.Value.FirstName,
               LastName = u.Value.LastName
           }).ToList()
       };

       return response;
    }
}