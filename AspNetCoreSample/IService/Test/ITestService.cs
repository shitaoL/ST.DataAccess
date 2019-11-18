using ST.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreSample
{
    public interface ITestService : IBaseService
    {
        TestDto GetById(string id);
        Task<TestDto> GetByIdAsync(string id);
        Task<PagedListDto<Test>> GetPagedList();
    }
}
