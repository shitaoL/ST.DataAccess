using ST.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreSample
{
    public interface ITestRepository : IRepository<Test>
    {
        //can add your other interface here
        //...
    }
}
