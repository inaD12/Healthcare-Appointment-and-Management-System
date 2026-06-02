namespace Shared.Application.Abstractions;

public interface IDateTimeProvider
{
	DateTime UtcNow { get; }
	DateTime GetUtcNow(int seconds);
}
