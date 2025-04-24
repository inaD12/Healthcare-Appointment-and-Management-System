using AutoMapper;
using Shared.Application.Abstractions;

namespace Shared.Application.Helpers;

public class HAMSMapper : IHAMSMapper
{
	private readonly IMapper _mapper;

	public HAMSMapper(IMapper mapper)
	{
		_mapper = mapper;
	}

	public T Map<T>(object source)
	{
		return _mapper.Map<T>(source);
	}

	public void Map(object source, object destination)
	{
		_mapper.Map(source, destination);
	}
}
