using Autofac;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace AspNetCoreSample
{
    public class AutofacModule : Autofac.Module
    {
        //重写Autofac管道Load方法，在这里注入
        protected override void Load(ContainerBuilder builder)
        {
            //仓储层和IUnitOfWork已经在ST.DataAccess库中自动注入,仓储层只要继承IRepository接口都会自动注入,例如:ITestRepository : IRepository<Test>

            //以程序集方式注入AspNetCoreSample下的所有的以Service结尾的类
            builder.RegisterAssemblyTypes(GetAssemblyByName("AspNetCoreSample")).Where(x => x.Name.EndsWith("Service")).AsImplementedInterfaces().InstancePerDependency();

            //你也可以一个一个注入:
            //builder.RegisterType<BaseService>().As<IBaseService>().InstancePerDependency();
            //builder.RegisterType<TestService>().As<ITestService>().InstancePerDependency();
            //...

        }

        public static Assembly GetAssemblyByName(string assemblyName)
        {
            return Assembly.Load(assemblyName);
        }
    }
}
