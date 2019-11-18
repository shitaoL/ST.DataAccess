
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using ST.DataAccess;

namespace AspNetCoreSample
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
