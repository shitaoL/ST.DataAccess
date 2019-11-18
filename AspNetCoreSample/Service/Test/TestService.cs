using AutoMapper;
using ST.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace AspNetCoreSample
{
    public class TestService : BaseService, ITestService
    {
        private IMapper _mapper;

        private readonly ITestRepository _testRepository;
        //private readonly IRepository<Test> _repository; 如果你没有ITestRepository接口,你也可以用这种方式

        private readonly IUnitOfWork _unitOfWork;

        public TestService(IMapper mapper, ITestRepository testRepository, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _testRepository = testRepository;
            _unitOfWork = unitOfWork;
        }

        public TestDto GetById(string id)
        {
            var ety = _testRepository.GetById(id);

            return _mapper.Map<TestDto>(ety);
        }

        public async Task<TestDto> GetByIdAsync(string id)
        {
            var ety = await _testRepository.GetByIdAsync(id);

            return _mapper.Map<TestDto>(ety);
        }

        public async Task<PagedListDto<Test>> GetPagedList()
        {
            var tests = await _testRepository.GetAll().ToPagedListAsync(1, 10);
            return _mapper.Map<PagedListDto<Test>>(tests);
        }
    }
}
