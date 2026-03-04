using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Users.Application.Features.Users.Mappers;
using Users.Application.Features.Users.Models;
using Users.Domain.Abstractions.Repositories;
using Users.Domain.Utilities;

namespace Users.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler(IUserRepository userRepository)
	: IQueryHandler<GetAllUsersQuery, UserPaginatedQueryViewModel>
{
	public async Task<Result<UserPaginatedQueryViewModel>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
	{
		var userPagedListQuery = request.ToPagedListQuery();
		var usersPagedList = await userRepository.GetAllAsync(userPagedListQuery, cancellationToken);
		if (usersPagedList == null)
			return Result<UserPaginatedQueryViewModel>.Failure(ResponseList.NoUsersFound);

		var userPaginatedQueryViewModel = usersPagedList.ToQueryViewModel();
		return Result<UserPaginatedQueryViewModel>.Success(userPaginatedQueryViewModel);
	}
}
