
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace ST.DataAccess.Test
{
    public class TestRepository : EfCoreRepository<Test>, ITestRepository
    {
        public TestRepository(DbContext context) : base(context)
        {

        }

        //implement your interface(ITestRepository) here
        //...
    }
}
