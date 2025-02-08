using Microsoft.Extensions.DependencyInjection;

namespace Shared.Domain.Abstractions;

public interface IDatabaseInitializer
{
	Task ApplyMigrationsAsync(IServiceScope scope);
}