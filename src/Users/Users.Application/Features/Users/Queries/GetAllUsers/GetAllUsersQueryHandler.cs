using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Users.Application.Features.Users.Mappers;
using Users.Application.Features.Users.Models;
using Users.Domain.Infrastructure.Abstractions.Repositories;
using Users.Domain.Responses;

namespace Users.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, UserPaginatedQueryViewModel>
{
	private readonly IUserRepository _userRepository;

	public GetAllUsersQueryHandler(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<Result<UserPaginatedQueryViewModel>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
	{
		var userPagedListQuery = request.ToPagedListQuery();
		var usersPagedList = await _userRepository.GetAllAsync(userPagedListQuery, cancellationToken);
		if (usersPagedList == null)
			return Result<UserPaginatedQueryViewModel>.Failure(ResponseList.NoUsersFound);

		var userPaginatedQueryViewModel = usersPagedList.ToQueryViewModel();
		return Result<UserPaginatedQueryViewModel>.Success(userPaginatedQueryViewModel);
	}
}
