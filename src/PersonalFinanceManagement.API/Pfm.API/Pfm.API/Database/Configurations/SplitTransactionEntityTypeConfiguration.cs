using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalFinanceManagement.API.Database.Entities;

namespace PersonalFinanceManagement.API.Database.Configurations
{
    public class SplitTransactionEntityTypeConfiguration : IEntityTypeConfiguration<SplitTransactionEntity>
    {
        public void Configure(EntityTypeBuilder<SplitTransactionEntity> builder)
        {
            builder.ToTable("SplitTransactions");
            builder.HasKey(st => new { st.Id, st.Catcode});
        }
    }
}
