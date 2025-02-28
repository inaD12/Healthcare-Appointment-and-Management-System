using Shared.Application.Abstractions;
using Shared.Infrastructure.Abstractions;

namespace Shared.Application.UnitTests.Utilities;

public class BaseSharedUnitTest
{
	protected IHAMSMapper HAMSMapper { get; }
	protected IUnitOfWork UnitOfWork { get; }

	public BaseSharedUnitTest(IHAMSMapper hAMSMapper, IUnitOfWork unitOfWork)
	{
		HAMSMapper = hAMSMapper;
		UnitOfWork = unitOfWork;
	}
}
