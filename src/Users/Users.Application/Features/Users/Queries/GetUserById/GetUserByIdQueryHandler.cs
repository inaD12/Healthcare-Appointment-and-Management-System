using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Users.Application.Features.Users.Mappers;
using Users.Application.Features.Users.Models;
using Users.Domain.Infrastructure.Abstractions.Repositories;
using Users.Domain.Responses;

namespace Users.Application.Features.Users.Queries.GetById;

public sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserQueryViewModel>
{
	private readonly IUserRepository _userRepository;

	public GetUserByIdQueryHandler(IUserRepository userRepository)
	{
		_userRepository = userRepository;
	}

	public async Task<Result<UserQueryViewModel>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByIdAsync(request.Id, cancellationToken);
		if (user == null)
			return Result<UserQueryViewModel>.Failure(ResponseList.UserNotFound);

		var userQueryViewModel = user.ToQueryViewModel();
		return Result<UserQueryViewModel>.Success(userQueryViewModel);
	}
}
