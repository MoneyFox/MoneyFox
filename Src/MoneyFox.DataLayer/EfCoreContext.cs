using Microsoft.EntityFrameworkCore;
using MoneyFox.DataLayer.Configurations;
using MoneyFox.DataLayer.Entities;
using MoneyFox.Foundation;
using System;
using System.IO;

namespace MoneyFox.DataLayer
{
    /// <summary>
    ///     Represents the data context of the application
    /// </summary>
    public class EfCoreContext : DbContext
    {
        public EfCoreContext()
        {
        }

        public EfCoreContext(DbContextOptions options) : base(options) { }

        public DbSet<Account> Accounts { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<RecurringPayment> RecurringPayments { get; set; }
        public DbSet<Category> Categories { get; set; }

        /// <summary>
        ///     The Path to the db who shall be opened
        /// </summary>
        public static string DbPath { get; set; }

        private const string databaseName = "moneyfox3.db";

        /// <summary>
        ///     This is called when before the db is access.
        ///     Set DbPath before, so that we use here what db we have to use.
        /// </summary>
        /// <param name="optionsBuilder">Optionbuilder who is used.</param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            String databasePath = "";
            switch (ExecutingPlatform.Current)
            {
                case AppPlatform.iOS:
                    SQLitePCL.Batteries_V2.Init();
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "..", "Library", databaseName); ;
                    break;
                case AppPlatform.Android:
                    databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Personal), databaseName);
                    break;

                case AppPlatform.UWP:
                    databasePath = databaseName;
                    break;

                default:
                    throw new NotImplementedException("Platform not supported");
            }

            // Specify that we will use sqlite and the path of the database here
            optionsBuilder.UseSqlite($"Filename={databasePath}");
        }



        /// <summary>
        ///     Called when the models are created.
        ///     Enables to configure advanced settings for the models.
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ThrowIfNull(modelBuilder);

            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        }

        private void ThrowIfNull(ModelBuilder modelBuilder) { if (modelBuilder == null) throw new ArgumentNullException(nameof(modelBuilder));  }
    }
}
