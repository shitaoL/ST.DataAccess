using AutoMapper;
using ST.DataAccess;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreSample
{
    public class TestService : BaseService, ITestService
    {
        private readonly ITestRepository _testRepository; //private readonly IRepository<Test> _repository; 如果你没有ITestRepository接口,你也可以用这种方式
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAsyncQueryableExecuter _asyncQueryableExecuter;
        private readonly IMapper _mapper;

        public TestService(ITestRepository testRepository, IUnitOfWork unitOfWork, IAsyncQueryableExecuter asyncQueryableExecuter, IMapper mapper)
        {
            _testRepository = testRepository;
            _unitOfWork = unitOfWork;
            _asyncQueryableExecuter = asyncQueryableExecuter;
            _mapper = mapper;
        }

        public TestDto GetById(string id)
        {
            var ety = _testRepository.GetById(id);

            var dto = _mapper.Map<TestDto>(ety);

            return dto;
        }

        public async Task<TestDto> GetByIdAsync(string id)
        {
            var ety = await _testRepository.GetByIdAsync(id);

            return _mapper.Map<TestDto>(ety);
        }

        public PagedResultDto<TestDto> GetPagedList(int pageIndex, int pageSize)
        {
            var qry = _testRepository.GetAll().WhereIf(true, x => x.Name.StartsWith("Jack"));

            //用WhereIf添加过滤条件
            qry = qry.WhereIf(true, x => x.Name.StartsWith("Jack"));
            qry = qry.WhereIf(true, x => x.Age > 20);


            var totalCount = qry.Count();
            var list = qry.OrderBy(x => x.Name).PageBy(pageIndex, pageSize).ToList();

            var items = _mapper.Map<List<TestDto>>(list);

            return new PagedResultDto<TestDto>(totalCount, items);
        }


        public async Task<PagedResultDto<TestDto>> GetPagedListAsync(int pageIndex, int pageSize)
        {
            var qry = _testRepository.GetAll().WhereIf(true, x => x.Name.StartsWith("Jack"));

            //用WhereIf添加过滤条件
            qry = qry.WhereIf(true, x => x.Name.StartsWith("Jack"));
            qry = qry.WhereIf(true, x => x.Age > 20);


            var totalCount = await _asyncQueryableExecuter.CountAsync(qry);

            var list = await _asyncQueryableExecuter.ToListAsync(qry.PageBy(pageIndex, pageSize));

            var items = _mapper.Map<List<TestDto>>(list);

            return new PagedResultDto<TestDto>(totalCount, items);
        }

        public async Task<TestDto> Create(CreateTestInput input)
        {
            var ety = _mapper.Map<Test>(input);

            _testRepository.Insert(ety);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TestDto>(ety);
        }

        public async Task<TestDto> Update(UpdateTestInput input)
        {
            var ety = await _testRepository.GetByIdAsync(input.Id);

            _mapper.Map(input, ety);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<TestDto>(ety);
        }

    }
}
