using System.Reflection;
using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core.Manager;
using MoneyManager.Core.ViewModels;
using MoneyManager.DataAccess;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MvvmCross.Plugins.Sqlite;

namespace MoneyManager.Core
{
    public class App : MvxApplication
    {
        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            CreateDatabase();

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes(typeof(AccountDataAccess).GetTypeInfo().Assembly)
                .EndingWith("DataAccess")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes(typeof(AccountDataAccess).GetTypeInfo().Assembly)
                .EndingWith("DataAccess")
                .AsTypes()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("Repository")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("Manager")
                .AsTypes()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("ViewModel")
                .AsTypes()
                .RegisterAsLazySingleton();

            Mvx.Resolve<RecurringTransactionManager>().CheckRecurringTransactions();
            Mvx.Resolve<TransactionManager>().ClearTransactions();

            // Start the app with the Main View Model.
            RegisterAppStart<MainViewModel>();
        }

        private void CreateDatabase()
        {
            var factory = Mvx.Resolve<IMvxSqliteConnectionFactory>();

            using (var db = factory.GetConnection("moneyfox.sqlite"))
            {
                db.CreateTable<Account>();
                db.CreateTable<FinancialTransaction>();
                db.CreateTable<RecurringTransaction>();
                db.CreateTable<Category>();
            }
        }
    }
}