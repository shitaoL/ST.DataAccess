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
        PagedResultDto<TestDto> GetPagedList(int pageIndex, int pageSize);
        Task<PagedResultDto<TestDto>> GetPagedListAsync(int pageIndex, int pageSize);
        Task<TestDto> Create(CreateTestInput input);
        Task<TestDto> Update(UpdateTestInput input);

    }
}
