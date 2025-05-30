using Microsoft.EntityFrameworkCore;

namespace Shared.Domain.Models;

public class PagedList<T>
{
	public PagedList(IEnumerable<T> items, int page, int pageSize, int totalCount)
	{
		Items = items;
		Page = page;
		PageSize = pageSize;
		TotalCount = totalCount;
	}

	public IEnumerable<T> Items { get; }
	public int Page { get; }
	public int PageSize { get; }
	public int TotalCount { get; }
	public bool HasNextPage => Page * PageSize < TotalCount;
	public bool HasPreviousPage => Page > 1;

	public static async Task<PagedList<T>> CreateAsync(IQueryable<T> source, int page, int pageSize, CancellationToken cancellationToken)
	{
		var count = await source.CountAsync(cancellationToken);
		var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken);
		return new PagedList<T>(items, page, pageSize, count);
	}
};
