using Shared.Application.Abstractions;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Users.Application.Features.Users.Models;
using Users.Domain.Infrastructure.Abstractions.Repositories;
using Users.Domain.Responses;

namespace Users.Application.Features.Users.Queries.GetById;

public sealed class GetUserByIdQueryHandler : IQueryHandler<GetUserByIdQuery, UserQueryViewModel>
{
	private readonly IUserRepository _userRepository;
	private readonly IHAMSMapper _mapper;

	public GetUserByIdQueryHandler(IUserRepository userRepository, IHAMSMapper mapper)
	{
		_userRepository = userRepository;
		_mapper = mapper;
	}

	public async Task<Result<UserQueryViewModel>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
	{
		var user = await _userRepository.GetByIdAsync(request.Id);
		if (user == null)
			return Result<UserQueryViewModel>.Failure(ResponseList.UserNotFound);

		var userQueryViewModel = _mapper.Map<UserQueryViewModel>(user);
		return Result<UserQueryViewModel>.Success(userQueryViewModel);
	}
}
