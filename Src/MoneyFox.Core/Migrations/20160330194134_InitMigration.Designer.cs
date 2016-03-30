using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using MoneyFox.Core.DataAccess;

namespace MoneyFox.Core.Migrations
{
    [DbContext(typeof(MoneyFoxDataContext))]
    [Migration("20160330194134_InitMigration")]
    partial class InitMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348");

            modelBuilder.Entity("MoneyFox.Foundation.Model.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("CurrentBalance");

                    b.Property<string>("Iban");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Note");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MoneyFox.Foundation.Model.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MoneyFox.Foundation.Model.Payment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<int?>("CategoryId");

                    b.Property<int>("ChargedAccountId");

                    b.Property<bool>("ClearPaymentNow");

                    b.Property<DateTime>("Date");

                    b.Property<bool>("IsCleared");

                    b.Property<bool>("IsRecurring");

                    b.Property<bool>("IsTransfer");

                    b.Property<string>("Note");

                    b.Property<int>("RecurringPaymentId");

                    b.Property<int>("TargetAccountId");

                    b.Property<int>("Type");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MoneyFox.Foundation.Model.RecurringPayment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("Amount");

                    b.Property<int?>("CategoryId");

                    b.Property<int>("ChargedAccountId");

                    b.Property<DateTime>("EndDate");

                    b.Property<bool>("IsEndless");

                    b.Property<string>("Note");

                    b.Property<int>("Recurrence");

                    b.Property<DateTime>("StartDate");

                    b.Property<int>("TargetAccountId");

                    b.Property<int>("Type");

                    b.HasKey("Id");
                });

            modelBuilder.Entity("MoneyFox.Foundation.Model.Payment", b =>
                {
                    b.HasOne("MoneyFox.Foundation.Model.Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("MoneyFox.Foundation.Model.Account")
                        .WithMany()
                        .HasForeignKey("ChargedAccountId");

                    b.HasOne("MoneyFox.Foundation.Model.RecurringPayment")
                        .WithMany()
                        .HasForeignKey("RecurringPaymentId");

                    b.HasOne("MoneyFox.Foundation.Model.Account")
                        .WithMany()
                        .HasForeignKey("TargetAccountId");
                });

            modelBuilder.Entity("MoneyFox.Foundation.Model.RecurringPayment", b =>
                {
                    b.HasOne("MoneyFox.Foundation.Model.Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("MoneyFox.Foundation.Model.Account")
                        .WithMany()
                        .HasForeignKey("ChargedAccountId");

                    b.HasOne("MoneyFox.Foundation.Model.Account")
                        .WithMany()
                        .HasForeignKey("TargetAccountId");
                });
        }
    }
}
