using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PersonalFinanceManagement.API.Database.Entities;

namespace PersonalFinanceManagement.API.Database.Configurations
{
    public class CategoryEntityTypeConfiguration : IEntityTypeConfiguration<CategoryEntity>
    {
        public void Configure(EntityTypeBuilder<CategoryEntity> builder)
        {

            // PK
            builder.ToTable("categories");
            builder.HasKey(t => t.Code);

            // Columns
            builder.Property<string>("Name")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property<string>("ParentCode");

        }
    }
}
