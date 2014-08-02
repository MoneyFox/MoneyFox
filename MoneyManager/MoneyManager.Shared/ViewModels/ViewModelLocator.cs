using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.ViewModels.Data;
using MoneyManager.ViewModels.Views;

namespace MoneyManager.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<AccountViewModel>();
            SimpleIoc.Default.Register<CategoryViewModel>();
            SimpleIoc.Default.Register<GroupViewModel>();
            SimpleIoc.Default.Register<RecurrenceTransactionViewModel>();
            SimpleIoc.Default.Register<SettingViewModel>();
            SimpleIoc.Default.Register<TransactionViewModel>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AccountListViewModel>();
            SimpleIoc.Default.Register<AddAccountViewModel>();
            SimpleIoc.Default.Register<AddAccountUserControlViewModel>();
            SimpleIoc.Default.Register<AddTransactionViewModel>();
            SimpleIoc.Default.Register<StatisticViewModel>();
            SimpleIoc.Default.Register<SettingsCategoryViewModel>();
        }

        #region DataAccess

        public AccountViewModel AccountViewModel
        {
            get { return ServiceLocator.Current.GetInstance<AccountViewModel>(); }
        }

        public CategoryViewModel CategoryViewModel
        {
            get { return ServiceLocator.Current.GetInstance<CategoryViewModel>(); }
        }

        public GroupViewModel GroupViewModel
        {
            get { return ServiceLocator.Current.GetInstance<GroupViewModel>(); }
        }

        public RecurrenceTransactionViewModel RecurrenceTransactionViewModel
        {
            get { return ServiceLocator.Current.GetInstance<RecurrenceTransactionViewModel>(); }
        }

        public TransactionViewModel TransactionViewModel
        {
            get { return ServiceLocator.Current.GetInstance<TransactionViewModel>(); }
        }

        public SettingViewModel Settings
        {
            get { return ServiceLocator.Current.GetInstance<SettingViewModel>(); }
        }

        public SettingsCategoryViewModel SettingsCategory
        {
            get { return ServiceLocator.Current.GetInstance<SettingsCategoryViewModel>(); }
        }

        #endregion DataAccess

        #region Views

        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        public AccountListViewModel AccountList
        {
            get { return ServiceLocator.Current.GetInstance<AccountListViewModel>(); }
        }

        public AddAccountViewModel AddAccount
        {
            get { return ServiceLocator.Current.GetInstance<AddAccountViewModel>(); }
        }

        public AddAccountUserControlViewModel AddAccountUserControl
        {
            get { return ServiceLocator.Current.GetInstance<AddAccountUserControlViewModel>(); }
        }

        public AddTransactionViewModel AddTransaction
        {
            get { return ServiceLocator.Current.GetInstance<AddTransactionViewModel>(); }
        }

        public StatisticViewModel Statistic
        {
            get { return ServiceLocator.Current.GetInstance<StatisticViewModel>(); }
        }

        #endregion Views
    }
}