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
        public DbSet<SplitTransactionEntity> SplitTransactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TransactionEntityTypeConfigurations());
            modelBuilder.ApplyConfiguration(new CategoryEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new SplitTransactionEntityTypeConfiguration());

            // 1 - Many (Category -> Transactions)
            modelBuilder.Entity<TransactionEntity>()
                .HasOne(p => p.Category)
                .WithMany(b => b.Transactions)
                .HasForeignKey(p => p.Catcode)
                //.OnDelete(DeleteBehavior.Cascade) ?
                ;

            modelBuilder.Entity<SplitTransactionEntity>()
                .HasOne(p => p.Transaction)
                .WithMany(b => b.SplitTransactions)
                .HasForeignKey(p => p.Id)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<SplitTransactionEntity>()
                .HasOne(p => p.Category)
                .WithMany(b => b.SplitTransactions)
                .HasForeignKey(p => p.Catcode);

            base.OnModelCreating(modelBuilder);
        }
    }
}
