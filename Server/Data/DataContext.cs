using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using AllinOrder_Shahaf_Ofir_Snir.Shared.Entities;

namespace AllinOrder_Shahaf_Ofir_Snir.Server.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Item> Items { get; set; }
        public object Question { get; internal set; }
    }
}
