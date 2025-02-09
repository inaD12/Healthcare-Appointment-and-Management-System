using Appointments.Domain.Abstractions.UseCase;

namespace Appointments.Infrastructure.Jobs;

public class UseCaseExecutor
{
	private readonly IUseCase _useCase;

	public UseCaseExecutor(IUseCase useCase)
	{
		_useCase = useCase;
	}

	public async Task ExecuteAsync()
	{
		await _useCase.ExecuteAsync();
	}
}
