using Contracts.Results;
using MediatR;

namespace Contracts.Abstractions.Messaging;

public interface IQueryHandler<TQuery, TResponse>
	: IRequestHandler<TQuery, Result<TResponse>>
	where TQuery : IQuery<TResponse>
{
}
