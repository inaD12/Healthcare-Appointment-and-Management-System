using Microsoft.Extensions.DependencyInjection;

namespace Shared.Application.IntegrationTests.Utilities;

public class BaseSharedIntegrationTest
{
    protected IServiceScope ServiceScope { get; }

	public BaseSharedIntegrationTest(IServiceScope serviceScope)
	{
		ServiceScope = serviceScope;
	}
}
