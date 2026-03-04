using Shared.Domain.Abstractions;

namespace Shared.Application.UnitTests.Utilities;

public class BaseSharedUnitTest
{
	protected IUnitOfWork UnitOfWork { get; }
	protected CancellationToken CancellationToken { get; }

	public BaseSharedUnitTest(IUnitOfWork unitOfWork)
	{
		UnitOfWork = unitOfWork;
		CancellationToken = CancellationToken.None;
	}
}
