using System.Data.Common;
using Dapper;
using Shared.Application.Authorization;
using Shared.Application.Data;
using Shared.Domain.Abstractions.Messaging;
using Shared.Domain.Results;
using Users.Domain.Utilities;

namespace Users.Application.Features.Users.Queries.GetUserPermissions;

internal sealed class GetUserPermissionsQueryHandler(IDbConnectionFactory dbConnectionFactory)
    : IQueryHandler<GetUserPermissionsQuery, PermissionsResponse>
{
    public async Task<Result<PermissionsResponse>> Handle(
        GetUserPermissionsQuery request,
        CancellationToken cancellationToken)
    {
        await using DbConnection connection = await dbConnectionFactory.OpenConnectionAsync();

        const string sql =
            $"""
             SELECT DISTINCT
                 u."Id" AS {nameof(UserPermission.UserId)},
                 rp."PermissionCode" AS {nameof(UserPermission.Permission)}
             FROM "Users" u
             JOIN "user_roles" ur ON ur."UserId" = u."Id"
             JOIN "role_permissions" rp ON rp."RoleName" = ur."role_name"
             WHERE u."IdentityId" = @IdentityId
             """;
        List<UserPermission> permissions = (await connection.QueryAsync<UserPermission>(sql, request)).AsList();

        if (!permissions.Any())
        {
            return Result<PermissionsResponse>.Failure(ResponseList.UserNotFound);
        }

        return Result<PermissionsResponse>.Success(new PermissionsResponse(permissions[0].UserId, permissions.Select(p => p.Permission).ToHashSet()));
    }

    internal sealed class UserPermission
    {
        internal string UserId { get; init; }

        internal string Permission { get; init; }
    }
}
