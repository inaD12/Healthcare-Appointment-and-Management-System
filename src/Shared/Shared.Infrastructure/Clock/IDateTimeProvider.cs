namespace Shared.Infrastructure.Clock;

public interface IDateTimeProvider
{
	DateTime UtcNow { get; }
}
