using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ST.DataAccess;

namespace AspNetCoreSample.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TestsController : ControllerBase
    {
        //注意：正常项目应该还有一个IService和Service层()，Service层里面才会引用仓储类(IUnitOfWork,ITestRepository),然后Controller层引用(注入)IService层，这里测试没有些Service层
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITestRepository _testRepository;
        private readonly ILogger<TestsController> _logger;

        public TestsController(IUnitOfWork unitOfWork, ITestRepository testRepository, ILogger<TestsController> logger)
        {
            _unitOfWork = unitOfWork;
            _testRepository = testRepository;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var tests = await _testRepository.GetAll().ToPagedListAsync(1, 10);
            return Ok(tests);
        }


    }
}
