using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.API.Database.Configurations;
using PersonalFinanceManagement.API.Database.Entities;
using System.Reflection;


namespace PersonalFinanceManagement.API.Database
{
    public class TransactionsDbContext : DbContext
    {
        public TransactionsDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<TransactionEntity> Transactions { get; set; }
        public DbSet<CategoryEntity> Categories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TransactionEntityTypeConfigurations());
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());

            // 1 - Many (Category -> Transactions)
            modelBuilder.Entity<TransactionEntity>()
                .HasOne(p => p.Category)
                .WithMany(b => b.Transactions)
                .HasForeignKey(p => p.Catcode)
                //.OnDelete(DeleteBehavior.Cascade)?
                ;


            base.OnModelCreating(modelBuilder);
        }
    }
}
