using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyModel;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using System.Linq;

namespace ST.DataAccess
{
    public class BaseDbContext : DbContext
    {
        protected BaseDbContext() { }
        public BaseDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assemblies = GetCurrentPathAssembly();
            foreach (var assembly in assemblies)
            {
                var entityTypes = assembly.GetTypes()
                    .Where(type => !string.IsNullOrWhiteSpace(type.Namespace))
                    .Where(type => type.IsClass)
                    .Where(type => type.BaseType != null)
                    .Where(type => typeof(IEntity).IsAssignableFrom(type));

                foreach (var entityType in entityTypes)
                {
                    if (modelBuilder.Model.FindEntityType(entityType) != null)
                        continue;
                    modelBuilder.Model.AddEntityType(entityType);
                }
            }
            base.OnModelCreating(modelBuilder);
        }


        private List<Assembly> GetCurrentPathAssembly()
        {
            var dlls = DependencyContext.Default.CompileLibraries.Where(x => !x.Name.StartsWith("Microsoft") && !x.Name.StartsWith("System")).ToList();

            return dlls.Where(x => x.Type == "project").Select(x => Assembly.Load(x.Name)).ToList();
        }
    }
}
