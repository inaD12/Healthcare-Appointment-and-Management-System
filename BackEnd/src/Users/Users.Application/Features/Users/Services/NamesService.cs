using MediatR;
using Shared.Application.Abstractions;
using Shared.Application.Models;
using Shared.Domain.Results;
using Users.Application.Features.Users.Abstractions;
using Users.Application.Features.Users.Queries.GetUserById;
using Users.Domain.Utilities;

namespace Users.Application.Features.Users.Services;

public class NamesService(ISender sender, IUserRepository repository): INamesService, IBatchNamesService
{
    public async Task<Result<NamesResponse>> GetUserNamesAsync(string userId, CancellationToken cancellationToken = default)
    {
        var name = await repository.GetNameByIdAsync(userId, cancellationToken);
        
        if (name is null)
        {
            return Result<NamesResponse>.Failure(ResponseList.UserNotFound);
        }
        
        return Result<NamesResponse>.Success(name);
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