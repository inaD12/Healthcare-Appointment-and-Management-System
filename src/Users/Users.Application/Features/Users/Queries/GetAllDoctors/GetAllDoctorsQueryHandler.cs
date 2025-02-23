using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Users.Application.Features.Managers.Interfaces;
using Users.Domain.Entities;

namespace Users.Application.Features.Users.Queries.GetAllDoctors;

public sealed class GetAllDoctorsQueryHandler
	: IQueryHandler<GetAllDoctorsQuery, IEnumerable<User>>
{
	private readonly IRepositoryManager _repositoryManager;

	public GetAllDoctorsQueryHandler(IRepositoryManager repositoryManager)
	{
		_repositoryManager = repositoryManager;
	}

	public async Task<Result<IEnumerable<User>>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
	{
		var res = await _repositoryManager.User.GetAllDoctorsAsync();

		if (res.IsFailure)
			return Result<IEnumerable<User>>.Failure(res.Response);

		return Result<IEnumerable<User>>.Success(res.Value!);
	}
}
