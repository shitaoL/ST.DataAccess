using System;
using System.Collections.Generic;
using System.Text;

namespace ST.DataAccess
{
    /// <summary>
    /// page model
    /// </summary>
    /// <typeparam name="T">The type of the data to page</typeparam>
    public interface IPagedList<out T>
    {
        /// <summary>
        /// page index
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// page size
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// items total count
        /// </summary>
        int Total { get; }


        /// <summary>
        /// page total count
        /// </summary>
        int PageTotal { get; }

        /// <summary>
        /// data items
        /// </summary>
        IEnumerable<T> Items { get; }
    }
}
