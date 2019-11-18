using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ST.DataAccess;

namespace AspNetCoreSample
{
    public class BlogRepository : EfCoreRepository<Blog>, IBlogRepository
    {
        public BlogRepository(DbContext context) : base(context)
        {

        }

        //implement your interface(IBlogRepository) here
        //...
    }
}
