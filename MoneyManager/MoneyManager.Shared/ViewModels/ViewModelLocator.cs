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
            SimpleIoc.Default.Register<MainViewModel>();
            SimpleIoc.Default.Register<SettingsViewModel>();

            SimpleIoc.Default.Register<AccountViewModel>();
        }

        #region DataAccess

        public AccountViewModel AccountViewModel
        {
            get { return ServiceLocator.Current.GetInstance<AccountViewModel>(); }
        }

        #endregion DataAccess

        #region Views

        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        public SettingsViewModel Settings
        {
            get { return ServiceLocator.Current.GetInstance<SettingsViewModel>(); }
        }

        #endregion Views
    }
}