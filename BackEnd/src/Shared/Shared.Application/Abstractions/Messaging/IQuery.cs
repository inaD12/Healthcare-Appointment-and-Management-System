using MediatR;
using Shared.Domain.Results;

namespace Shared.Application.Abstractions.Messaging;

public interface IQuery<Tresponse> : IRequest<Result<Tresponse>>
{
}
