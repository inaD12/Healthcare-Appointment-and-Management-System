using MediatR;
using Shared.Domain.Results;

namespace Shared.Domain.Abstractions.Messaging;

public interface ICommand : IRequest<Result>
{
}

public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}
