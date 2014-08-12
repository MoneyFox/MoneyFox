using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.ViewModels.Views;

namespace MoneyManager.ViewModels
{
    public class ViewModelLocator
    {
        static ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<AccountDataAccess>();
            SimpleIoc.Default.Register<CategoryDataAccess>();
            SimpleIoc.Default.Register<GroupDataAccess>();
            SimpleIoc.Default.Register<SettingDataAccess>();
            SimpleIoc.Default.Register<TransactionDataAccess>();

            SimpleIoc.Default.Register<MainPageViewModel>();
            SimpleIoc.Default.Register<AccountListUserControlViewModel>();
            SimpleIoc.Default.Register<AddAccountViewModel>();
            SimpleIoc.Default.Register<AddAccountUserControlViewModel>();
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

        public SettingDataAccess Settings
        {
            get { return ServiceLocator.Current.GetInstance<SettingDataAccess>(); }
        }

        #endregion DataAccess

        #region Views

        public MainPageViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainPageViewModel>(); }
        }

        public AccountListUserControlViewModel AccountListControl
        {
            get { return ServiceLocator.Current.GetInstance<AccountListUserControlViewModel>(); }
        }

        public AddAccountUserControlViewModel AddAccountControl
        {
            get { return ServiceLocator.Current.GetInstance<AddAccountUserControlViewModel>(); }
        }

        public AddAccountViewModel AddAccountView
        {
            get { return ServiceLocator.Current.GetInstance<AddAccountViewModel>(); }
        }

        #endregion Views
    }
}