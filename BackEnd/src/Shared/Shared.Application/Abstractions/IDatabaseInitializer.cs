using Microsoft.Extensions.DependencyInjection;

namespace Shared.Application.Abstractions;

public interface IDatabaseInitializer
{
	Task ApplyMigrationsAsync(IServiceScope scope);
}