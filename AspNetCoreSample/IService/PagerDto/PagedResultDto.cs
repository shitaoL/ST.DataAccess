using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreSample
{
    public class PagedResultDto<T>
    {
        public PagedResultDto(int totalCount, IEnumerable<T> items)
        {
            TotalCount = totalCount;
            Items = items;
        }
        public int TotalCount { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
