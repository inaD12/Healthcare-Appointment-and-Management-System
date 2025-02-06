using Contracts.Results;
using MediatR;

namespace Contracts.Abstractions.Messaging;

public interface IQuery<Tresponse> : IRequest<Result<Tresponse>>
{
}
