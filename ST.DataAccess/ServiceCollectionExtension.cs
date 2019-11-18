using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ST.DataAccess
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddDataAccess(this IServiceCollection services,
            Action<DbContextOptionsBuilder> options)
        {
            services.AddDbContext<DbContext, BaseDbContext>(options);

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AutoDI(typeof(IRepository<>));

            services.AddScoped(typeof(IRepository<>), typeof(EfCoreRepository<>));

            return services;
        }

        public static IServiceCollection AddDataAccess<T>(this IServiceCollection services,
            Action<DbContextOptionsBuilder> options) where T : BaseDbContext
        {
            services.AddDbContext<DbContext, T>(options);

            services.AddScoped(typeof(IRepository<>), typeof(EfCoreRepository<>));

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AutoDI(typeof(IRepository<>));


            return services;
        }


        private static IServiceCollection AutoDI(this IServiceCollection services, Type baseType)
        {
            var assemblies = DependencyContext.Default.CompileLibraries
                            .Where(x => !x.Name.StartsWith("Microsoft") && !x.Name.StartsWith("System"))
                            .Where(x => x.Type == "project").Select(x => Assembly.Load(x.Name));

            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes().Where(type => type.IsClass && type.BaseType != null && type.HasImplementedRawGeneric(baseType));

                foreach (var type in types)
                {
                    var interfaces = type.GetInterfaces();

                    var interfaceType = interfaces.FirstOrDefault(x => x.Name == $"I{type.Name}");
                    if (interfaceType == null)
                    {
                        interfaceType = type;
                    }
                    ServiceDescriptor serviceDescriptor = new ServiceDescriptor(interfaceType, type, ServiceLifetime.Scoped);
                    if (!services.Contains(serviceDescriptor))
                    {
                        services.Add(serviceDescriptor);
                    }
                }
            }
            return services;
        }

        private static bool HasImplementedRawGeneric(this Type type, Type generic)
        {
            if (type == null) throw new ArgumentNullException(nameof(type));
            if (generic == null) throw new ArgumentNullException(nameof(generic));

            var isTheRawGenericType = type.GetInterfaces().Any(IsTheRawGenericType);

            if (isTheRawGenericType) return true;

            while (type != null && type != typeof(object))
            {
                isTheRawGenericType = IsTheRawGenericType(type);
                if (isTheRawGenericType) return true;
                type = type.BaseType;
            }
            return false;

            bool IsTheRawGenericType(Type test) => generic == (test.IsGenericType ? test.GetGenericTypeDefinition() : test);
        }

    }
}
