namespace Shared.Application.Abstractions;

public interface IHAMSMapper
{
	void Map(object source, object destination);

	T Map<T>(object source);
}
