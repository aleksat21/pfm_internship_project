﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using PersonalFinanceManagement.API.Database;

#nullable disable

namespace PersonalFinanceManagement.API.Migrations
{
    [DbContext(typeof(TransactionsDbContext))]
    partial class TransactionsDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.7")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("PersonalFinanceManagement.API.Database.Entities.CategoryEntity", b =>
                {
                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ParentCode")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Code");

                    b.ToTable("categories", (string)null);
                });

            modelBuilder.Entity("PersonalFinanceManagement.API.Database.Entities.SplitTransactionEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Catcode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.HasKey("Id", "Catcode");

                    b.HasIndex("Catcode");

                    b.ToTable("SplitTransactions", (string)null);
                });

            modelBuilder.Entity("PersonalFinanceManagement.API.Database.Entities.TransactionEntity", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<double>("Amount")
                        .HasColumnType("float");

                    b.Property<string>("BeneficiaryName")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<string>("Catcode")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Direction")
                        .IsRequired()
                        .HasMaxLength(1)
                        .HasColumnType("nvarchar(1)");

                    b.Property<string>("Kind")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Mcc")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Catcode");

                    b.ToTable("transactions", (string)null);
                });

            modelBuilder.Entity("PersonalFinanceManagement.API.Database.Entities.SplitTransactionEntity", b =>
                {
                    b.HasOne("PersonalFinanceManagement.API.Database.Entities.CategoryEntity", "Category")
                        .WithMany("SplitTransactions")
                        .HasForeignKey("Catcode")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("PersonalFinanceManagement.API.Database.Entities.TransactionEntity", "Transaction")
                        .WithMany("SplitTransactions")
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");

                    b.Navigation("Transaction");
                });

            modelBuilder.Entity("PersonalFinanceManagement.API.Database.Entities.TransactionEntity", b =>
                {
                    b.HasOne("PersonalFinanceManagement.API.Database.Entities.CategoryEntity", "Category")
                        .WithMany("Transactions")
                        .HasForeignKey("Catcode");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("PersonalFinanceManagement.API.Database.Entities.CategoryEntity", b =>
                {
                    b.Navigation("SplitTransactions");

                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("PersonalFinanceManagement.API.Database.Entities.TransactionEntity", b =>
                {
                    b.Navigation("SplitTransactions");
                });
#pragma warning restore 612, 618
        }
    }
}
