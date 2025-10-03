using Shared.Application.Abstractions;
using Shared.Domain.Abstractions;

namespace Shared.Application.UnitTests.Utilities;

public class BaseSharedUnitTest
{
	protected IHAMSMapper HAMSMapper { get; }
	protected IUnitOfWork UnitOfWork { get; }
	protected CancellationToken CancellationToken { get; }

	public BaseSharedUnitTest(IHAMSMapper hamsMapper, IUnitOfWork unitOfWork)
	{
		HAMSMapper = hamsMapper;
		UnitOfWork = unitOfWork;
		CancellationToken = CancellationToken.None;
	}
}
