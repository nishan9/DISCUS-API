using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DISCUS_API
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<EventEntity> EventEntity { get; set; }
    }
}
