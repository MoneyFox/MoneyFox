using Windows.ApplicationModel;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Core;
using MoneyManager.Core.DataAccess;
using MoneyManager.Core.Helper;
using MoneyManager.Core.Manager;
using MoneyManager.Core.Repositories;
using MoneyManager.Core.ViewModels;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using MoneyManager.Windows.Services;
using MoneyManager.Windows.Views;
using SQLite.Net.Interop;
using SQLite.Net.Platform.WinRT;
using IDialogService = MoneyManager.Foundation.OperationContracts.IDialogService;

namespace MoneyManager.Windows
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (DesignMode.DesignModeEnabled)
            {
                return;
            }

            //Prepare Platform specifics for creating the database and a DbHelper
            SimpleIoc.Default.Register<ISQLitePlatform, SQLitePlatformWinRT>();
            SimpleIoc.Default.Register<IDatabasePath, DatabasePath>();
            SimpleIoc.Default.Register<IDbHelper, DbHelper>();

            //DataAccess
            SimpleIoc.Default.Register<IDataAccess<Account>, AccountDataAccess>();
            SimpleIoc.Default.Register<IDataAccess<Category>, CategoryDataAccess>();
            SimpleIoc.Default.Register<IDataAccess<FinancialTransaction>, TransactionDataAccess>();
            SimpleIoc.Default.Register<IDataAccess<RecurringTransaction>, RecurringTransactionDataAccess>();
            SimpleIoc.Default.Register<SettingDataAccess>();

            //Plattform specfic Logic
            SimpleIoc.Default.Register<IDialogService, DialogService>();
            SimpleIoc.Default.Register<IAppInformation, AppInformation>();

            //Logic
            SimpleIoc.Default.Register<IUserNotification, UserNotification>();
            SimpleIoc.Default.Register<IBackupService, OneDriveBackupService>();
            SimpleIoc.Default.Register(CreateNavigationService);
            SimpleIoc.Default.Register<Utilities>();
            SimpleIoc.Default.Register<Backup>();

            //Repositories
            SimpleIoc.Default.Register<ITransactionRepository, TransactionRepository>();
            SimpleIoc.Default.Register<IRepository<RecurringTransaction>, RecurringTransactionRepository>();
            SimpleIoc.Default.Register<IRepository<Account>, AccountRepository>();
            SimpleIoc.Default.Register<IRepository<Category>, CategoryRepository>();

            //Datadependent Logic
            SimpleIoc.Default.Register<RepositoryManager>();
            SimpleIoc.Default.Register<TransactionManager>();

            //ViewModels
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AddAccountViewModel>();
            SimpleIoc.Default.Register<AccountListUserControlViewModel>();
            SimpleIoc.Default.Register<AddTransactionViewModel>();
            SimpleIoc.Default.Register<BalanceViewModel>();
            SimpleIoc.Default.Register<CategoryListViewModel>();
            SimpleIoc.Default.Register<TransactionListViewModel>();
            SimpleIoc.Default.Register<TileSettingsViewModel>();
            SimpleIoc.Default.Register<SettingDefaultsViewModel>();
            SimpleIoc.Default.Register<StatisticViewModel>();
            SimpleIoc.Default.Register<BackupViewModel>();
        }

        #region Logic

        private static INavigationService CreateNavigationService()
        {
            var navigationService = new PageNavigationService();

            navigationService.Configure("AddAccountView", typeof (AddAccountView));
            navigationService.Configure("AddTransactionView", typeof (AddTransactionView));

            return navigationService;
        }

        #endregion

        // TODO: Remove this, shouldn't be needed.

        #region DataAccess

        public IRepository<Account> AccountRepository => ServiceLocator.Current.GetInstance<IRepository<Account>>();

        public IRepository<Category> CategoryRepository => ServiceLocator.Current.GetInstance<IRepository<Category>>();

        public ITransactionRepository TransactionRepository
            => ServiceLocator.Current.GetInstance<ITransactionRepository>();

        public IRepository<RecurringTransaction> RecurringTransactionRepository
            => ServiceLocator.Current.GetInstance<IRepository<RecurringTransaction>>();

        public SettingDataAccess SettingDataAccess => ServiceLocator.Current.GetInstance<SettingDataAccess>();

        #endregion DataAccess

        #region Views

        public MainViewModel MainViewModel => ServiceLocator.Current.GetInstance<MainViewModel>();

        public AddAccountViewModel AddAccountViewModel => ServiceLocator.Current.GetInstance<AddAccountViewModel>();

        public AccountListUserControlViewModel AccountListUserControlViewModel
            => ServiceLocator.Current.GetInstance<AccountListUserControlViewModel>();

        public AddTransactionViewModel AddTransactionViewModel
            => ServiceLocator.Current.GetInstance<AddTransactionViewModel>();

        public BalanceViewModel BalanceViewModel => ServiceLocator.Current.GetInstance<BalanceViewModel>();

        public CategoryListViewModel CategoryListViewModel
            => ServiceLocator.Current.GetInstance<CategoryListViewModel>();

        public TransactionListViewModel TransactionListViewModel
            => ServiceLocator.Current.GetInstance<TransactionListViewModel>();

        public TileSettingsViewModel TileSettingsViewModel
            => ServiceLocator.Current.GetInstance<TileSettingsViewModel>();

        public SettingDefaultsViewModel SettingDefaultsViewModel
            => ServiceLocator.Current.GetInstance<SettingDefaultsViewModel>();

        public StatisticViewModel StatisticViewModel => ServiceLocator.Current.GetInstance<StatisticViewModel>();

        public BackupViewModel BackupViewModel => ServiceLocator.Current.GetInstance<BackupViewModel>();

        #endregion Views
    }
}