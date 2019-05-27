using CtsContestWeb.Db.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace CtsContestWeb.Db
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Solution> Solutions { get; set; }
        public DbSet<Purchase> Purchases { get; set; }
        public DbSet<GivenPurchase> GivenPurchases { get; set; }
        public DbSet<ContactInfo> ContactInformation { get; set; }
        public DbSet<Duel> Duels { get; set; }
        public DbSet<DuelSolution> DuelSolutions { get; set; }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<TaskInput> TaskInputs { get; set; }
        public DbSet<TaskOutput> TaskOutputs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Purchase>()
                .HasIndex(p => new { p.UserEmail, p.PrizeId })
                .IsUnique(true);

            modelBuilder.Entity<Solution>()
                .HasIndex(p => new { p.UserEmail, p.TaskId })
                .IsUnique(true);

            modelBuilder.Entity<User>()
                .HasIndex(p => new { p.Email })
                .IsUnique(true);

            modelBuilder.Entity<Duel>()
                .HasIndex(p => new { p.FirstPlayerEmail, p.SecondPlayerEmail, p.TaskId })
                .IsUnique(true);

            modelBuilder.Entity<DuelSolution>()
                .HasIndex(p => new { p.UserEmail, p.DuelId })
                .IsUnique(true);

            //modelBuilder.Entity<CompetitionSolution>()
            //    .HasIndex(p => new { p.UserEmail, p.Competition.TaskId })
            //    .IsUnique(true);
        }

        public override int SaveChanges()
        {
            SetMetadata();
            return base.SaveChanges();
        }

        private void SetMetadata()
        {
            foreach (var entity in ChangeTracker.Entries().Where(e => e.State == EntityState.Added || e.State == EntityState.Modified))
            {
                var auditable = entity.Entity as IAuditable;
                if (auditable != null)
                {
                    auditable.Created = DateTime.Now;
                }
            }
        }
    }
}
