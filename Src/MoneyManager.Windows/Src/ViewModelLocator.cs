using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business;
using MoneyManager.Business.Helper;
using MoneyManager.Business.Manager;
using MoneyManager.Business.Repositories;
using MoneyManager.Business.Services;
using MoneyManager.Business.ViewModels;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using MoneyManager.Windows.Views;

namespace MoneyManager.Windows
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            DatabaseLogic.CreateDatabase();

            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            //DataAccess
            SimpleIoc.Default.Register<IDataAccess<Account>, AccountDataAccess>();
            SimpleIoc.Default.Register<IDataAccess<Category>, CategoryDataAccess>();
            SimpleIoc.Default.Register<IDataAccess<FinancialTransaction>, TransactionDataAccess>();
            SimpleIoc.Default.Register<IDataAccess<RecurringTransaction>, RecurringTransactionDataAccess>();
            SimpleIoc.Default.Register<SettingDataAccess>();

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

            //ViewModels
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AddAccountViewModel>();
            SimpleIoc.Default.Register<AccountListUserControlViewModel>();
            SimpleIoc.Default.Register<AddTransactionViewModel>();
            SimpleIoc.Default.Register<BalanceViewModel>();
            SimpleIoc.Default.Register<CategoryListViewModel>();
            SimpleIoc.Default.Register<TransactionListViewModel>();
            SimpleIoc.Default.Register<TileSettingsViewModel>();
            SimpleIoc.Default.Register<GeneralSettingViewModel>();
            SimpleIoc.Default.Register<SettingDefaultsViewModel>();
            SimpleIoc.Default.Register<StatisticViewModel>();
            SimpleIoc.Default.Register<BackupViewModel>();
        }

        #region Logic

        private static INavigationService CreateNavigationService()
        {
            var navigationService = new NavigationService();
            //Just an example
            //navigationService.Configure("LicenseView", typeof (LicenseView));

            navigationService.Configure("AddAccountView", typeof(AddAccountView));

            return navigationService;
        }

        #endregion

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

        public AccountListUserControlViewModel AccountListUserControlViewModel => ServiceLocator.Current.GetInstance<AccountListUserControlViewModel>();

        public AddTransactionViewModel AddTransactionViewModel => ServiceLocator.Current.GetInstance<AddTransactionViewModel>();

        public BalanceViewModel BalanceViewModel => ServiceLocator.Current.GetInstance<BalanceViewModel>();

        public CategoryListViewModel CategoryListViewModel => ServiceLocator.Current.GetInstance<CategoryListViewModel>();

        public TransactionListViewModel TransactionListViewModel => ServiceLocator.Current.GetInstance<TransactionListViewModel>();

        public TileSettingsViewModel TileSettingsViewModel => ServiceLocator.Current.GetInstance<TileSettingsViewModel>();

        public GeneralSettingViewModel GeneralSettingViewModel => ServiceLocator.Current.GetInstance<GeneralSettingViewModel>();

        public SettingDefaultsViewModel SettingDefaultsViewModel => ServiceLocator.Current.GetInstance<SettingDefaultsViewModel>();

        public StatisticViewModel StatisticViewModel => ServiceLocator.Current.GetInstance<StatisticViewModel>();

        public BackupViewModel BackupViewModel => ServiceLocator.Current.GetInstance<BackupViewModel>();

        #endregion Views
    }
}