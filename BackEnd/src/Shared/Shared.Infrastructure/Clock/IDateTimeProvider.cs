namespace Shared.Infrastructure.Clock;

public interface IDateTimeProvider
{
	DateTime UtcNow { get; }
	DateTime GetUtcNow(int seconds);
}
