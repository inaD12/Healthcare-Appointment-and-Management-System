﻿using Shared.Application.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Users.Application.Features.Users.Models;
using Users.Domain.Abstractions.Repositories;
using Users.Domain.Models;

namespace Users.Application.Features.Users.Queries.GetAllUsers;

public class GetAllUsersQueryHandler : IQueryHandler<GetAllUsersQuery, UserPaginatedQueryViewModel>
{
	private readonly IUserRepository _userRepository;
	private readonly IHAMSMapper _hamsMapper;

	public GetAllUsersQueryHandler(IUserRepository userRepository, IHAMSMapper hamsMapper)
	{
		_userRepository = userRepository;
		_hamsMapper = hamsMapper;
	}

	public async Task<Result<UserPaginatedQueryViewModel>> Handle(GetAllUsersQuery request, CancellationToken cancellationToken)
	{
		var userPagedListQuery = _hamsMapper.Map<UserPagedListQuery>(request);
		var usersPagedList = await _userRepository.GetAllAsync(userPagedListQuery, cancellationToken);

		var userPaginatedQueryViewModel = _hamsMapper.Map<UserPaginatedQueryViewModel>(usersPagedList);
		return Result<UserPaginatedQueryViewModel>.Success(userPaginatedQueryViewModel);
	}
}
