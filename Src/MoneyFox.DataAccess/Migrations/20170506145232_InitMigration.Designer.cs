using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MoneyFox.DataAccess.Migrations
{
    [DbContext(typeof(ApplicationContext))]
    [Migration("20170506145232_InitMigration")]
    partial class InitMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("MoneyFox.DataAccess.Entities.AccountEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("CurrentBalance");

                    b.Property<string>("Iban");

                    b.Property<bool>("IsExcluded");

                    b.Property<bool>("IsOverdrawn");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Note");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("MoneyFox.DataAccess.Entities.CategoryEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Notes");

                    b.HasKey("Id");

                    b.HasIndex("Name");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("MoneyFox.DataAccess.Entities.PaymentEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<int?>("CategoryId");

                    b.Property<int>("ChargedAccountId");

                    b.Property<DateTime>("Date");

                    b.Property<bool>("IsCleared");

                    b.Property<bool>("IsRecurring");

                    b.Property<string>("Note");

                    b.Property<int?>("RecurringPaymentId");

                    b.Property<int?>("TargetAccountId");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ChargedAccountId");

                    b.HasIndex("RecurringPaymentId");

                    b.HasIndex("TargetAccountId");

                    b.ToTable("Payments");
                });

            modelBuilder.Entity("MoneyFox.DataAccess.Entities.RecurringPaymentEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<int?>("CategoryId");

                    b.Property<int>("ChargedAccountId");

                    b.Property<DateTime?>("EndDate");

                    b.Property<bool>("IsEndless");

                    b.Property<string>("Note");

                    b.Property<int>("Recurrence");

                    b.Property<DateTime>("StartDate");

                    b.Property<int?>("TargetAccountId");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.HasIndex("ChargedAccountId");

                    b.HasIndex("TargetAccountId");

                    b.ToTable("RecurringPayments");
                });

            modelBuilder.Entity("MoneyFox.DataAccess.Entities.PaymentEntity", b =>
                {
                    b.HasOne("MoneyFox.DataAccess.Entities.CategoryEntity", "Category")
                        .WithMany("Payments")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("MoneyFox.DataAccess.Entities.AccountEntity", "ChargedAccount")
                        .WithMany("ChargedPayments")
                        .HasForeignKey("ChargedAccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MoneyFox.DataAccess.Entities.RecurringPaymentEntity", "RecurringPayment")
                        .WithMany("RelatedPayments")
                        .HasForeignKey("RecurringPaymentId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("MoneyFox.DataAccess.Entities.AccountEntity", "TargetAccount")
                        .WithMany("TargetedPayments")
                        .HasForeignKey("TargetAccountId")
                        .OnDelete(DeleteBehavior.SetNull);
                });

            modelBuilder.Entity("MoneyFox.DataAccess.Entities.RecurringPaymentEntity", b =>
                {
                    b.HasOne("MoneyFox.DataAccess.Entities.CategoryEntity", "Category")
                        .WithMany("RecurringPayments")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.HasOne("MoneyFox.DataAccess.Entities.AccountEntity", "ChargedAccount")
                        .WithMany("ChargedRecurringPayments")
                        .HasForeignKey("ChargedAccountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("MoneyFox.DataAccess.Entities.AccountEntity", "TargetAccount")
                        .WithMany("TargetedRecurringPayments")
                        .HasForeignKey("TargetAccountId")
                        .OnDelete(DeleteBehavior.SetNull);
                });
        }
    }
}
