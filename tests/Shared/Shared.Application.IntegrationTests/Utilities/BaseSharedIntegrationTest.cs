using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Shared.Application.IntegrationTests.Utilities;

public class BaseSharedIntegrationTest
{
	protected IServiceScope ServiceScope { get; }
	protected CancellationToken CancellationToken { get; }
	protected ISender Sender { get; }
	public BaseSharedIntegrationTest(IServiceScope serviceScope)
	{
		ServiceScope = serviceScope;
		CancellationToken = new CancellationToken();
		Sender = ServiceScope.ServiceProvider.GetRequiredService<ISender>();
	}
}
