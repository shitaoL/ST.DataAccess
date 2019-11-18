using ST.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreSample
{
    public interface IBlogRepository : IRepository<Blog>
    {
        //can add your other interface here
        //...
    }
}
