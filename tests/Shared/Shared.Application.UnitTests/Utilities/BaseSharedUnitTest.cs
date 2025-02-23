using Shared.Application.Abstractions;

namespace Shared.Application.UnitTests.Utilities;

public class BaseSharedUnitTest
{
	protected IHAMSMapper HAMSMapper { get; }

	public BaseSharedUnitTest(IHAMSMapper hAMSMapper)
	{
		HAMSMapper = hAMSMapper;
	}
}
