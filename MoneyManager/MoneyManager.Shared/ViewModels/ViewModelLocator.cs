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
            SimpleIoc.Default.Register<SettingViewModel>();
            SimpleIoc.Default.Register<TransactionViewModel>();

            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<AccountListUserControlViewModel>();
            SimpleIoc.Default.Register<AddAccountViewModel>();
            SimpleIoc.Default.Register<AddAccountUserControlViewModel>();
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

        public TransactionViewModel TransactionViewModel
        {
            get { return ServiceLocator.Current.GetInstance<TransactionViewModel>(); }
        }

        public SettingViewModel Settings
        {
            get { return ServiceLocator.Current.GetInstance<SettingViewModel>(); }
        }

        #endregion DataAccess

        #region Views

        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
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