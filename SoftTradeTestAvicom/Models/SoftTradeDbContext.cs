using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftTradeTestAvicom.Models
{
    public class SoftTradeDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=.\SQLEXPRESS;Initial Catalog=SoftTrade;integrated security=True;MultipleActiveResultSets=True;App=EntityFramework");
            base.OnConfiguring(optionsBuilder);
        }

        public DbSet<Manager> Managers { get; set; }
    }
}
