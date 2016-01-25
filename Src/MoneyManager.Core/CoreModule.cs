using System.Reflection;
using Autofac;
using MoneyManager.Core.Authentication;
using MoneyManager.Core.ViewModels;
using MoneyManager.Core.ViewModels.CategoryList;
using MoneyManager.Core.ViewModels.Dialogs;
using MoneyManager.Core.ViewModels.SettingViews;
using MoneyManager.Core.ViewModels.Statistics;
using MoneyManager.DataAccess;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MvvmCross.Platform;
using Module = Autofac.Module;

namespace MoneyManager.Core
{
    public class CoreModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<SqliteConnectionCreator>().As<ISqliteConnectionCreator>();

            builder.RegisterType<PasswordStorage>().As<IPasswordStorage>();
            builder.RegisterType<Session>().AsSelf();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Service"))
                .AsImplementedInterfaces()
                .SingleInstance();

            //We have to register them seperatly, otherwise it wasn't able to resolve them.
            //TODO: Find a better way to do this.
            builder.RegisterType<AccountDataAccess>().As<IDataAccess<Account>>();
            builder.RegisterType<PaymentDataAccess>().As<IDataAccess<Payment>>();
            builder.RegisterType<RecurringPaymentDataAccess>().As<IDataAccess<RecurringPayment>>();
            builder.RegisterType<CategoryDataAccess>().As<IDataAccess<Category>>();
            builder.RegisterType<SettingDataAccess>().AsSelf();

            // This is needed for SettingDataAccess
            builder.RegisterAssemblyTypes(typeof(AccountDataAccess).GetTypeInfo().Assembly)
                .Where(t => t.Name.EndsWith("DataAccesss"))
                .AsSelf()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("Manager"))
                .AsImplementedInterfaces()
                .SingleInstance();

            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("ViewModel"))
                .AsImplementedInterfaces()
                .SingleInstance(); ;

            //TODO: Implement for each ViewModel an Interface
            builder.RegisterAssemblyTypes(ThisAssembly)
                .Where(t => t.Name.EndsWith("ViewModel"))
                .AsSelf()
                .SingleInstance();;
        }


        //Views
        public static MainViewModel MainView => Mvx.Resolve<MainViewModel>();
        public static AccountListViewModel AccountListView => Mvx.Resolve<AccountListViewModel>();
        public static PaymentListViewModel PaymentListView => Mvx.Resolve<PaymentListViewModel>();
        public static BackupViewModel BackupView => Mvx.Resolve<BackupViewModel>();
        public static BalanceViewModel BalanceView => Mvx.Resolve<BalanceViewModel>();
        public static ModifyAccountViewModel ModifyAccountView => Mvx.Resolve<ModifyAccountViewModel>();
        public static ModifyPaymentViewModel ModifyPaymentView => Mvx.Resolve<ModifyPaymentViewModel>();
        public static AboutViewModel AboutView => Mvx.Resolve<AboutViewModel>();
        public static RecurringPaymentListView RecurringPaymentListView => Mvx.Resolve<RecurringPaymentListView>();

        //CategoryList
        public static SelectCategoryListViewModel SelectCategoryListView => Mvx.Resolve<SelectCategoryListViewModel>();

        public static SettingsCategoryListViewModel SettingsCategoryListView
            => Mvx.Resolve<SettingsCategoryListViewModel>();

        //Dialogs
        public static CategoryDialogViewModel CategoryDialog => Mvx.Resolve<CategoryDialogViewModel>();

        public static SelectDateRangeDialogViewModel SelectDateRangeDialog
            => Mvx.Resolve<SelectDateRangeDialogViewModel>();

        //Statistics
        public static StatisticViewModel StatisticView => Mvx.Resolve<StatisticViewModel>();

        //Settings
        public static PasswordUserControlViewModel PasswordUserControls => Mvx.Resolve<PasswordUserControlViewModel>();
        public static SettingDefaultsViewModel SettingDefaultsView => Mvx.Resolve<SettingDefaultsViewModel>();
        public static TileSettingsViewModel TileSettingsView => Mvx.Resolve<TileSettingsViewModel>();
    }
}