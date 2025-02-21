using Mapster;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Users.Application.Features.Managers.Interfaces;
using Users.Domain.DTOs.Responses;

namespace Users.Application.Features.Users.Queries.GetAllDoctors;

public sealed class GetAllDoctorsQueryHandler
	: IQueryHandler<GetAllDoctorsQuery, IEnumerable<UserResponseDTO>>
{
	private readonly IRepositoryManager _repositoryManager;

	public GetAllDoctorsQueryHandler(IRepositoryManager repositoryManager)
	{
		_repositoryManager = repositoryManager;
	}

	public async Task<Result<IEnumerable<UserResponseDTO>>> Handle(GetAllDoctorsQuery request, CancellationToken cancellationToken)
	{
		var res = await _repositoryManager.User.GetAllDoctorsAsync();

		if (res.IsFailure)
			return Result<IEnumerable<UserResponseDTO>>.Failure(res.Response);

		var doctors = res.Value;

		var dtos = doctors.AsQueryable().ProjectToType<UserResponseDTO>();

		return Result<IEnumerable<UserResponseDTO>>.Success(dtos);
	}
}
