using Microsoft.Extensions.DependencyInjection;
using Users.Application.Features.Email.Helpers.Abstractions;
using Users.Application.Features.Managers.Interfaces;

namespace Users.Application.Features.Managers;

internal class FactoryManager : IFactoryManager
{
	private readonly IServiceProvider _serviceProvider;

	public FactoryManager(IServiceProvider serviceProvider)
	{
		_serviceProvider = serviceProvider;
	}

	public IEmailVerificationTokenFactory EmailTokenFactory => _serviceProvider.GetRequiredService<IEmailVerificationTokenFactory>();
	public IEmailVerificationLinkFactory EmailLinkFactory => _serviceProvider.GetRequiredService<IEmailVerificationLinkFactory>();
}
