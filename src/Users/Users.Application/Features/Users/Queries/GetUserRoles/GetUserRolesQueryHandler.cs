using System.Data.Common;
using Dapper;
using Shared.Application.Data;
using Shared.Application.Dtos;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Models;
using Shared.Domain.Results;
using Users.Domain.Responses;

namespace Users.Application.Features.Users.Queries.GetUserRoles;

internal sealed class GetUserRolesQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetUserRolesQuery, RolesResponse>
{
    public async Task<Result<RolesResponse>> Handle(
        GetUserRolesQuery request,
        CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT DISTINCT
                 u."Role" AS {nameof(UserRole.Role)},
             FROM "Users" u
             JOIN "user_roles" ur ON ur."UserId" = u."Id"
             WHERE u."Id" = @UserId
             """;
        List<UserRole> roles = (await connection.QueryAsync<UserRole>(sql, request)).AsList();

        if (!roles.Any())
        {
            return Result<RolesResponse>.Failure(ResponseList.UserNotFound);
        }

        return Result<RolesResponse>.Success(new RolesResponse(roles.Select(p => p.Role).ToHashSet()));
    }

    internal sealed class UserRole
    {
        internal string Role { get; init; }
    }
}
