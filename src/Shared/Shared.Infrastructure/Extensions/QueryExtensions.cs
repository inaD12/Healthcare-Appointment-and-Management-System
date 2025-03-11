using Shared.Domain.Enums;
using System.Linq.Expressions;

namespace Shared.Infrastructure.Extensions;

public static class QueryExtensions
{
	public static IOrderedQueryable<T> ApplySorting<T>(this IQueryable<T> query, string propertyName, SortOrder sortOrder)
	{
		var entityType = typeof(T);
		var orderByProperty = typeof(T).GetProperty(propertyName);

		var parameter = Expression.Parameter(entityType, "x");
		var propertyAccess = Expression.Property(parameter, orderByProperty!);
		var orderByExpression = Expression.Lambda<Func<T, object>>(Expression.Convert(propertyAccess, typeof(object)), parameter);

		if (sortOrder == SortOrder.DESC)
			return query.OrderByDescending(orderByExpression);

		return query.OrderBy(orderByExpression);
	}
}	