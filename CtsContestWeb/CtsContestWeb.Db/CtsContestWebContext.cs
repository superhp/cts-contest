using CtsContestWeb.Db.Entities;
using Microsoft.EntityFrameworkCore;

namespace CtsContestWeb.Db
{
    public class CtsContestWebContext : DbContext
    {
        public DbSet<Solution> Solutions { get; set; }
        public DbSet<Purchase> Purchases { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            => optionsBuilder
                .UseMySql(@"Server=localhost;database=ef;uid=root;pwd=123456;");
    }
}
