using System;
using System.Collections.Generic;
using System.Text;

namespace ST.DataAccess
{
    public class PagedList<T> : IPagedList<T>
    {
        public PagedList() { }

        public PagedList(IList<T> items, int pageIndex, int pageSize, int totalCount)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Total = totalCount;
            PageTotal = (int)Math.Ceiling(totalCount / (double)pageSize);
            Items = items;
        }

        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int Total { get; set; }
        public int PageTotal { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
