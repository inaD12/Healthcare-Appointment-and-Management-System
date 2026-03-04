using MediatR;
using Shared.Domain.Results;

namespace Shared.Domain.Abstractions.Messaging;

public interface IQuery<Tresponse> : IRequest<Result<Tresponse>>
{
}
