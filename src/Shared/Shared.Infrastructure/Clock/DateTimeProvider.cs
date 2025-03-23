namespace Shared.Infrastructure.Clock;

internal sealed class DateTimeProvider : IDateTimeProvider
{
	public DateTime UtcNow => DateTime.UtcNow;

	public DateTime GetUtcNow(int seconds) => DateTime.UtcNow.AddSeconds(seconds);

}
