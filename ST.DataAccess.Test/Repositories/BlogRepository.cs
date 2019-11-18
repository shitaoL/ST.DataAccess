using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ST.DataAccess.Test
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
