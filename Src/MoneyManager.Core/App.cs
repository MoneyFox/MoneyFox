using Cirrious.CrossCore;
using Cirrious.CrossCore.IoC;
using Cirrious.MvvmCross.ViewModels;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core
{
    public class App : MvxApplication
    {
        /// <summary>
        ///     Initializes this instance.
        /// </summary>
        public override void Initialize()
        {
            Mvx.RegisterType<IDbHelper, DbHelper>();

            CreatableTypes()
                .EndingWith("Service")
                .AsInterfaces()
                .RegisterAsLazySingleton();

            CreatableTypes()
                .EndingWith("DataAccess")
                .AsInterfaces()
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

            // Start the app with the Main View Model.
            RegisterAppStart<MainViewModel>();
        }

        //This properties are used to bind the view model to the datacontext directly in xaml (windows only)
        public MainViewModel MainViewModel => Mvx.Resolve<MainViewModel>();

        public AddAccountViewModel AddAccountViewModel => Mvx.Resolve<AddAccountViewModel>();

        public AccountListUserControlViewModel AccountListUserControlViewModel => Mvx.Resolve<AccountListUserControlViewModel>();

        public AddTransactionViewModel AddTransactionViewModel => Mvx.Resolve<AddTransactionViewModel>();

        public BalanceViewModel BalanceViewModel => Mvx.Resolve<BalanceViewModel>();

        public CategoryListViewModel CategoryListViewModel => Mvx.Resolve<CategoryListViewModel>();

        public TransactionListViewModel TransactionListViewModel => Mvx.Resolve<TransactionListViewModel>();

        public TileSettingsViewModel TileSettingsViewModel => Mvx.Resolve<TileSettingsViewModel>();

        public SettingDefaultsViewModel SettingDefaultsViewModel => Mvx.Resolve<SettingDefaultsViewModel>();

        public StatisticViewModel StatisticViewModel => Mvx.Resolve<StatisticViewModel>();

        public BackupViewModel BackupViewModel => Mvx.Resolve<BackupViewModel>();

        public SelectCategoryViewModel SelectCategoryViewModel => Mvx.Resolve<SelectCategoryViewModel>();
    }
}
