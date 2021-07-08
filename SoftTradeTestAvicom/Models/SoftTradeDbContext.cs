using Microsoft.EntityFrameworkCore;

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

        public DbSet<Product> Products { get; set; }

        public DbSet<Client> Clients { get; set; }
    }
}
