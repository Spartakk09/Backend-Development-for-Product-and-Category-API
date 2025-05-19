using Microsoft.EntityFrameworkCore;

namespace ProductsCategories.Services.Extensions
{
    public static class PaginationExtension
    {
        public static async Task<List<T>> ToPaginatedListAsync<T>(this IQueryable<T> query, int pageNumber, int pageSize)
        {
            var result  = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return result;
        }
    }
}
