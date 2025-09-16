//using BuildingBlocks.Pagination;
//using Microsoft.EntityFrameworkCore;

//public static class IQueryableExtensions
//{
//    public static async Task<PaginationResult<T>> PaginateAsync<T>(
//        this IQueryable<T> query,
//        int pageIndex,
//        int pageSize,
//        CancellationToken cancellationToken = default)
//        where T : class
//    {
//        if (pageIndex <= 0)
//            throw new ArgumentException("PageIndex must start from 1.", nameof(pageIndex));

//        var totalCount = await query.LongCountAsync(cancellationToken);

//        var items = await query
//            .Skip((pageIndex - 1) * pageSize) // 👈 para one-based index
//            .Take(pageSize)
//            .ToListAsync(cancellationToken);

//        return new PaginationResult<T>(
//            pageIndex,
//            pageSize,
//            totalCount,
//            items
//        );
//    }
//}
