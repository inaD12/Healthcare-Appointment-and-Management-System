using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Users.Application.Features.Users.Mappers;
using Users.Application.Features.Users.Models;
using Users.Domain.Abstractions.Repositories;
using Users.Domain.Utilities;

namespace Users.Application.Features.Users.Queries.GetUserById;

public sealed class GetUserByIdQueryHandler(IUserRepository userRepository)
	: IQueryHandler<GetUserByIdQuery, UserQueryViewModel>
{
	public async Task<Result<UserQueryViewModel>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
	{
		var user = await userRepository.GetByIdAsync(request.Id, cancellationToken);
		if (user == null)
			return Result<UserQueryViewModel>.Failure(ResponseList.UserNotFound);

		var userQueryViewModel = user.ToQueryViewModel();
		return Result<UserQueryViewModel>.Success(userQueryViewModel);
	}
}
