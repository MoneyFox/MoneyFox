using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.ViewModels;

namespace MoneyManager.Business.ViewModels
{
    internal class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<AccountDataAccess>();
            SimpleIoc.Default.Register<CategoryDataAccess>();
            SimpleIoc.Default.Register<GroupDataAccess>();
            SimpleIoc.Default.Register<SettingDataAccess>();
            SimpleIoc.Default.Register<TransactionDataAccess>();
            SimpleIoc.Default.Register<RecurringTransactionDataAccess>();
            SimpleIoc.Default.Register<StatisticDataAccess>();

            SimpleIoc.Default.Register<AddAccountViewModel>();
            SimpleIoc.Default.Register<AddTransactionViewModel>();
            SimpleIoc.Default.Register<TotalBalanceViewModel>();
            SimpleIoc.Default.Register<SelectCategoryViewModel>();
            SimpleIoc.Default.Register<TransactionListUserControlViewModel>();
            SimpleIoc.Default.Register<TileSettingsUserControlViewModel>();
            SimpleIoc.Default.Register<GeneralSettingUserControlViewModel>();
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

        public GroupDataAccess GroupDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<GroupDataAccess>(); }
        }

        public TransactionDataAccess TransactionDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>(); }
        }

        public RecurringTransactionDataAccess RecurringTransactionDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<RecurringTransactionDataAccess>(); }
        }

        public StatisticDataAccess StatisticDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<StatisticDataAccess>(); }
        }

        public SettingDataAccess Settings
        {
            get { return ServiceLocator.Current.GetInstance<SettingDataAccess>(); }
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

        public TransactionListUserControlViewModel TransactionListUserControlView
        {
            get { return ServiceLocator.Current.GetInstance<TransactionListUserControlViewModel>(); }
        }

        public TileSettingsUserControlViewModel TileSettingsUserControlView
        {
            get { return ServiceLocator.Current.GetInstance<TileSettingsUserControlViewModel>(); }
        }

        public GeneralSettingUserControlViewModel LanguageSettingUserControlView
        {
            get { return ServiceLocator.Current.GetInstance<GeneralSettingUserControlViewModel>(); }
        }

        #endregion Views
    }
}