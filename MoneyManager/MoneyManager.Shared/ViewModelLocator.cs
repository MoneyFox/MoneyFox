using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;
using MoneyManager.DataAccess.DataAccess;

namespace MoneyManager
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<AccountDataAccess>();
            SimpleIoc.Default.Register<CategoryDataAccess>();
            SimpleIoc.Default.Register<TransactionDataAccess>();
            SimpleIoc.Default.Register<RecurringTransactionDataAccess>();

            SimpleIoc.Default.Register<AddAccountViewModel>();
            SimpleIoc.Default.Register<AddTransactionViewModel>();
            SimpleIoc.Default.Register<TotalBalanceViewModel>();
            SimpleIoc.Default.Register<SelectCategoryViewModel>();
            SimpleIoc.Default.Register<SelectCurrencyViewModel>();
            SimpleIoc.Default.Register<TransactionListViewModel>();
            SimpleIoc.Default.Register<TileSettingsViewModel>();
            SimpleIoc.Default.Register<GeneralSettingViewModel>();
            SimpleIoc.Default.Register<CategorySettingsViewModel>();
            SimpleIoc.Default.Register<StatisticViewModel>();
        }

        #region DataAccess

        public AccountDataAccess AccountDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
        }

        public CategoryDataAccess CategoryDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<CategoryDataAccess>(); }
        }

        public TransactionDataAccess TransactionDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>(); }
        }

        public RecurringTransactionDataAccess RecurringTransactionDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<RecurringTransactionDataAccess>(); }
        }

        #endregion DataAccess

        #region Views

        public AddAccountViewModel AddAccountView
        {
            get { return ServiceLocator.Current.GetInstance<AddAccountViewModel>(); }
        }

        public AddTransactionViewModel AddTransactionView
        {
            get { return ServiceLocator.Current.GetInstance<AddTransactionViewModel>(); }
        }

        public TotalBalanceViewModel TotalBalanceView
        {
            get { return ServiceLocator.Current.GetInstance<TotalBalanceViewModel>(); }
        }

        public SelectCategoryViewModel SelectCategoryView
        {
            get { return ServiceLocator.Current.GetInstance<SelectCategoryViewModel>(); }
        }

        public SelectCurrencyViewModel SelectCurrencyView
        {
            get { return ServiceLocator.Current.GetInstance<SelectCurrencyViewModel>(); }
        }

        public TransactionListViewModel TransactionListView
        {
            get { return ServiceLocator.Current.GetInstance<TransactionListViewModel>(); }
        }

        public TileSettingsViewModel TileSettingsView
        {
            get { return ServiceLocator.Current.GetInstance<TileSettingsViewModel>(); }
        }

        public CategorySettingsViewModel CategorySettingsView
        {
            get { return ServiceLocator.Current.GetInstance<CategorySettingsViewModel>(); }
        }

        public GeneralSettingViewModel LanguageSettingView
        {
            get { return ServiceLocator.Current.GetInstance<GeneralSettingViewModel>(); }
        }

        public StatisticViewModel StatisticView
        {
            get { return ServiceLocator.Current.GetInstance<StatisticViewModel>(); }
        }

        #endregion Views
    }
}