using System;
using Microsoft.Data.Entity;
using Microsoft.Data.Entity.Infrastructure;
using Microsoft.Data.Entity.Metadata;
using Microsoft.Data.Entity.Migrations;
using MoneyFox.Core.DataAccess;

namespace MoneyFox.Core.Migrations
{
    [DbContext(typeof(MoneyFoxDataContext))]
    [Migration("20160401224634_AddConstraints")]
    partial class AddConstraints
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.0-rc1-16348");

            modelBuilder.Entity("MoneyFox.Core.DatabaseModels.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<double>("CurrentBalance");

                    b.Property<string>("Iban");

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<string>("Note");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "Accounts");
                });

            modelBuilder.Entity("MoneyFox.Core.DatabaseModels.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "Categories");
                });

            modelBuilder.Entity("MoneyFox.Core.DatabaseModels.Payment", b =>
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

                    b.Property<int>("RecurringPaymentId");

                    b.Property<int>("TargetAccountId");

                    b.Property<int>("Type");

                    b.HasKey("Id");

                    b.HasAnnotation("Relational:TableName", "Payments");
                });

            modelBuilder.Entity("MoneyFox.Core.DatabaseModels.RecurringPayment", b =>
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

                    b.HasAnnotation("Relational:TableName", "RecurringPayments");
                });

            modelBuilder.Entity("MoneyFox.Core.DatabaseModels.RecurringPayment", b =>
                {
                    b.HasOne("MoneyFox.Core.DatabaseModels.Category")
                        .WithMany()
                        .HasForeignKey("CategoryId");

                    b.HasOne("MoneyFox.Core.DatabaseModels.Account")
                        .WithMany()
                        .HasForeignKey("ChargedAccountId");

                    b.HasOne("MoneyFox.Core.DatabaseModels.Account")
                        .WithMany()
                        .HasForeignKey("TargetAccountId");
                });
        }
    }
}
