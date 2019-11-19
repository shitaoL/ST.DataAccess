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
    [Route("[controller]/[action]")]
    public class TestsController : ControllerBase
    {
        private readonly ITestService _testService;

        public TestsController(ITestService testService)
        {
            _testService = testService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var tests = _testService.GetPagedList(1, 10);
            return Ok(tests);
        }


        [HttpPost]
        public IActionResult Create(CreateTestInput input)
        {
            var dto = _testService.Create(input);
            return Ok(dto);

        }

        [HttpPost]
        public IActionResult Update(UpdateTestInput input)
        {
            var dto = _testService.Update(input);
            return Ok(dto);
        }
    }
}
