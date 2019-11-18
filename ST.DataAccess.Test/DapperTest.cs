using Dapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ST.DataAccess.Test
{
    public class DapperTest
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ITestRepository _testRepository;

        public DapperTest()
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
        public async Task QueryAsync()
        {
            var tests = await _unitOfWork.QueryAsync<Test>("select * from Test");
            Assert.True(tests.Any());
        }


        [Fact]
        public async Task ExecuteAsync()
        {
            var test = new Test
            {
                Id = Guid.NewGuid().ToString(),
                Age = new Random().Next(1, 90),
                Name = "Jack" + new Random().Next(1, 90),
                CreationTime = DateTime.Now,
                Remark = "test" + new Random().Next(1, 90)
            };

            //note: sqlserver or mysql use "@", oracle user ":"
            await _unitOfWork.ExecuteAsync("insert into test(Id,Name,Age,CreationTime,Remark) values(:Id,:Name,:Age,:CreationTime,:Remark)", test);

            var newTest = await _unitOfWork.QueryAsync<Test>("select * from test where Id =:Id", new { Id = test.Id });

            Assert.True(test.Name == newTest.First().Name);
        }

        [Fact]
        public async Task GetConnection()
        {
            //可以直接使用dapper,拿到Connection就可以拿到所有的Dapper扩展方法
            var tests = await _unitOfWork.GetConnection().QueryAsync("select * from test");
            Assert.True(tests.Any());
        }

        [Fact]
        public async Task TransactionUseSaveChange()
        {
            var test1 = new Test
            {
                Id = Guid.NewGuid().ToString(),
                Age = new Random().Next(1, 90),
                Name = "Jack" + new Random().Next(1, 90),
                CreationTime = DateTime.Now,
                Remark = "test1" + new Random().Next(1, 90)
            };

            var test2 = new Test
            {
                Id = Guid.NewGuid().ToString(),
                Age = new Random().Next(1, 90),
                Name = "Jack" + new Random().Next(1, 90),
                CreationTime = DateTime.Now,
                Remark = "test2" + new Random().Next(1, 90)
            };

            await _testRepository.InsertAsync(test1);
            await _testRepository.InsertAsync(test2);
            await _unitOfWork.SaveChangesAsync();

            var newTest1 = await _unitOfWork.QueryAsync<Test>("select * from test where id =:Id", new { Id = test1.Id });
            var newTest2 = await _unitOfWork.QueryAsync<Test>("select * from test where id =:Id", new { Id = test2.Id });

            Assert.True(newTest1.Any() && newTest2.Any());
        }

        [Fact]
        public async Task Transaction()
        {
            var test1 = new Test
            {
                Id = Guid.NewGuid().ToString(),
                Age = new Random().Next(1, 90),
                Name = "Jack" + new Random().Next(1, 90),
                CreationTime = DateTime.Now,
                Remark = "tran test1" + new Random().Next(1, 90)
            };

            var test2 = new Test
            {
                Id = Guid.NewGuid().ToString(),
                Age = new Random().Next(1, 90),
                Name = "Jack" + new Random().Next(1, 90),
                CreationTime = DateTime.Now,
                Remark = "tran test2" + new Random().Next(1, 90)
            };

            using (var tran = _unitOfWork.BeginTransaction())
            {
                try
                {
                    await _unitOfWork.ExecuteAsync("insert into test(Id,Name,Age,CreationTime,Remark) values(:Id,:Name,:Age,:CreationTime,:Remark)", test1, tran);
                    await _unitOfWork.ExecuteAsync("insert into test(Id,Name,Age,CreationTime,Remark) values(:Id,:Name,:Age,:CreationTime,:Remark)", test2, tran);
                    throw new Exception();
                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                }
            }

            var newTest1 = await _unitOfWork.QueryAsync<Test>("select * from test where id =:Id", new { Id = test1.Id });
            var newTest2 = await _unitOfWork.QueryAsync<Test>("select * from test where id =:Id", new { Id = test2.Id });
            Assert.False(newTest1.Any() || newTest2.Any());
        }

        [Fact]
        public async Task HybridTransaction()
        {
            var test1 = new Test
            {
                Id = Guid.NewGuid().ToString(),
                Age = new Random().Next(1, 90),
                Name = "Jack" + new Random().Next(1, 90),
                CreationTime = DateTime.Now,
                Remark = "Hybrid test1" + new Random().Next(1, 90)
            };

            var test2 = new Test
            {
                Id = Guid.NewGuid().ToString(),
                Age = new Random().Next(1, 90),
                Name = "Jack" + new Random().Next(1, 90),
                CreationTime = DateTime.Now,
                Remark = "Hybrid test2" + new Random().Next(1, 90)
            };

            using (var tran = _unitOfWork.BeginTransaction())
            {
                try
                {
                    await _testRepository.InsertAsync(test1);
                    await _unitOfWork.SaveChangesAsync();

                    await _unitOfWork.ExecuteAsync("insert into test(Id,Name,Age,CreationTime,Remark) values(:Id,:Name,:Age,:CreationTime,:Remark)", test2, tran);
                    throw new Exception();
                    tran.Commit();
                }
                catch (Exception e)
                {
                    tran.Rollback();
                }
            }
            var newTest1 = await _unitOfWork.QueryAsync<Test>("select * from test where id =:id", new { Id = test1.Id });
            var newTest2 = await _unitOfWork.QueryAsync<Test>("select * from test where id =:id", new { Id = test2.Id });
            Assert.False(newTest1.Any() || newTest2.Any());
        }

    }
}
