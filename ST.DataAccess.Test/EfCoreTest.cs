using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;
using System.Threading.Tasks;
using System.Linq;

namespace ST.DataAccess.Test
{
    public class EfCoreTest
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITestRepository _testRepository;

        public EfCoreTest()
        {
             var services = new ServiceCollection();

            services.AddDataAccess<MyDbContext>(options => options.UseOracle("DATA SOURCE=192.168.100.84:1521/ROAPDB19;USER ID=dev;PASSWORD=123456;"));

            //or use below
            //services.AddDataAccess(options => options.UseOracle("DATA SOURCE=192.168.100.84:1521/ROAPDB19;USER ID=dev;PASSWORD=123456;"));

            var sp = services.BuildServiceProvider();
            _unitOfWork = sp.GetRequiredService<IUnitOfWork>();
            _testRepository = sp.GetRequiredService<ITestRepository>();
        }


        [Fact]
        public Test Insert()
        {
            var test = new Test
            {
                Id = Guid.NewGuid().ToString(),
                Age = new Random().Next(1, 90),
                Name = "Jack" + new Random().Next(1, 90),
                CreationTime = DateTime.Now,
                Remark = "test" + new Random().Next(1, 90)
            };
            _testRepository.Insert(test);
            _unitOfWork.SaveChanges();
            return test;
        }
        [Fact]
        public void GetById()
        {
            var newTest = Insert();
            var test = _testRepository.GetById(newTest.Id);
            Assert.True(test != null);
        }
        [Fact]
        public void PagedList()
        {
            var tests = _testRepository.GetAll().ToPagedList(1, 10);
            Assert.True(tests != null);
        }
        [Fact]
        public async Task PagedListAsync()
        {
            var tests = await _testRepository.GetAll().ToPagedListAsync(1, 10);
            Assert.True(tests != null);
        }


        [Fact]
        public List<Test> Inserts()
        {
            var tests = new List<Test>();
            for (int i = 0; i < 100; i++)
            {
                var test = new Test
                {
                    Id = Guid.NewGuid().ToString(),
                    Age = new Random().Next(1, 90),
                    Name = "Jack" + new Random().Next(1, 90),
                    CreationTime = DateTime.Now,
                    Remark = "test" + new Random().Next(1, 90)
                };
                tests.Add(test);
            }
            _testRepository.Insert(tests);
            _unitOfWork.SaveChanges();

            foreach (var item in tests)
            {
                var test = _testRepository.GetById(item.Id);
                if (test == null)
                {
                    Assert.True(false);
                }
            }
            return tests;
        }
        [Fact]
        public async Task InsertAsync()
        {
            var test = new Test
            {
                Id = Guid.NewGuid().ToString(),
                Age = new Random().Next(1, 90),
                Name = "Jack" + new Random().Next(1, 90),
                CreationTime = DateTime.Now,
                Remark = "test" + new Random().Next(1, 90)
            };
            await _testRepository.InsertAsync(test);
            await _unitOfWork.SaveChangesAsync();
        }
        [Fact]
        public async Task GetByIdAsync()
        {
            var newTest = Insert();

            var test = await _testRepository.GetByIdAsync(newTest.Id);
            Assert.True(test != null);
        }
        [Fact]
        public async Task InsertsAsync()
        {
            var tests = new List<Test>();
            for (int i = 0; i < 100; i++)
            {
                var test = new Test
                {
                    Id = Guid.NewGuid().ToString(),
                    Age = new Random().Next(1, 90),
                    Name = "Jack" + new Random().Next(1, 90),
                    CreationTime = DateTime.Now,
                    Remark = "test" + new Random().Next(1, 90)
                };
                tests.Add(test);
            }
            await _testRepository.InsertAsync(tests);
            await _unitOfWork.SaveChangesAsync();

            foreach (var item in tests)
            {
                var test = await _testRepository.GetByIdAsync(item.Id);
                if (test == null)
                {
                    Assert.True(false);
                }
            }
        }


        [Fact]
        public void Update()
        {
            var test = Insert();
            test.Name = Guid.NewGuid().ToString();
            _testRepository.Update(test);
            _unitOfWork.SaveChanges();
        }
        [Fact]
        public void Updates()
        {
            var tests = Inserts();

            tests.ForEach(x => x.Name = Guid.NewGuid().ToString());
            _testRepository.Update(tests);
            _unitOfWork.SaveChanges();

            foreach (var item in tests)
            {
                var test = _testRepository.GetById(item.Id);
                if (test.Name != item.Name)
                {
                    Assert.True(false);
                }
            }
        }
        [Fact]
        public void UpdateNoSelect()
        {
            var data = Insert();
            var test = new Test
            {
                Id = data.Id,
                Name = Guid.NewGuid().ToString()
            };
            _testRepository.Update(test, x => x.Name);
            _unitOfWork.SaveChanges();
            //这里不能使用 _testRepository.GetById(data.Id); 查询出来的结果和数据库不一致
            var newTest = _testRepository.GetAllNoTracking().First(x => x.Id == data.Id);
            Assert.True(newTest.Name == test.Name);
        }


        [Fact]
        public void Delete()
        {
            var test = Insert();

            _testRepository.Delete(test);
            _unitOfWork.SaveChanges();

            var newTest = _testRepository.GetAll().FirstOrDefault(x => x.Id == test.Id);
            Assert.True(newTest == null);
        }
        [Fact]
        public void Deletes()
        {
            var tests = new List<Test>();
            for (int i = 0; i < 100; i++)
            {
                var test = new Test
                {
                    Id = Guid.NewGuid().ToString(),
                    Age = new Random().Next(1, 90),
                    Name = "Jack" + new Random().Next(1, 90),
                    CreationTime = DateTime.Now,
                    Remark = "test" + new Random().Next(1, 90)
                };
                tests.Add(test);
            }
            _testRepository.Insert(tests);
            _unitOfWork.SaveChanges();

            _testRepository.Delete(tests);
            _unitOfWork.SaveChanges();

            foreach (var item in tests)
            {
                var Test = _testRepository.GetById(item.Id);
                if (Test != null)
                {
                    Assert.True(false);
                }
            }
        }
        [Fact]
        public void DeleteByLambda()
        {
            var data = Insert();

            _testRepository.Delete(x => x.Id == data.Id);
            _unitOfWork.SaveChanges();

            var newTest = _testRepository.GetAll().FirstOrDefault(x => x.Id == data.Id);
            Assert.True(newTest == null);
        }


    }
}
