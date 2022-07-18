using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalFinanceManagement.API.Database.Entities;

namespace PersonalFinanceManagement.API.Database.Configurations
{
    public class TransactionEntityTypeConfigurations : IEntityTypeConfiguration<TransactionEntity>
    {
        public void Configure(EntityTypeBuilder<TransactionEntity> builder)
        {
            builder.ToTable("transactions");
            builder.HasKey(t => t.Id);

            builder.Property<string>("BeneficiaryName")
                .HasMaxLength(128);

            builder.Property<DateTime>("Date")
               .IsRequired();

            builder.Property<Direction>("Direction")
               .HasConversion<string>()
               .HasMaxLength(1)
               .IsRequired();

            builder.Property<double>("Amount")
               .IsRequired();

            builder.Property<string>("Currency")
              .HasMaxLength(3)
              .IsRequired();

            builder.Property<Kind>("Kind")
              .HasConversion<string>()
              .IsRequired();
        }
    }
}
