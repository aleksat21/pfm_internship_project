using Microsoft.EntityFrameworkCore;
using PersonalFinanceManagement.API.Database.Configurations;
using PersonalFinanceManagement.API.Models;
using System.Reflection;


namespace PersonalFinanceManagement.API.Database
{
    public class TransactionsDbContext : DbContext
    {
        public TransactionsDbContext(DbContextOptions options) : base(options)
        {
        }
        public DbSet<Transaction> Transactions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new TransactionEntityTypeConfigurations());
            base.OnModelCreating(modelBuilder);
        }
    }
}
