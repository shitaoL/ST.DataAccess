using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ST.DataAccess
{
    public static class PagedListExtensions
    {
        public static PagedList<T> ToPagedList<T>(this IQueryable<T> query, int pageIndex, int pageSize)
        {
            if (pageIndex < 1) { throw new ArgumentOutOfRangeException(nameof(pageIndex)); }
            if (pageSize < 1) { throw new ArgumentOutOfRangeException(nameof(pageSize)); }

            var count = query.Count();

            var items = query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();

            return new PagedList<T>(items, pageIndex, pageSize, count);
        }

        public static async Task<PagedList<T>> ToPagedListAsync<T>(this IQueryable<T> query, int pageIndex, int pageSize, CancellationToken cancellationToken = default)
        {
            if (pageIndex < 1) { throw new ArgumentOutOfRangeException(nameof(pageIndex)); }
            if (pageSize < 1) { throw new ArgumentOutOfRangeException(nameof(pageSize)); }

            var count = await query.CountAsync(cancellationToken).ConfigureAwait(false);

            var items = await query.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToListAsync(cancellationToken).ConfigureAwait(false);

            return new PagedList<T>(items, pageIndex, pageSize, count);
        }

    }
}
