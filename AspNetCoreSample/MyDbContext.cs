using Microsoft.EntityFrameworkCore;
using ST.DataAccess;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetCoreSample
{
    public class MyDbContext : BaseDbContext
    {
        public MyDbContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //your code...
        }
    }
}
