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

        public SettingViewModel SettingViewModel
        {
            get { return ServiceLocator.Current.GetInstance<SettingViewModel>(); }
        }

        public TransactionViewModel TransactionViewModel
        {
            get { return ServiceLocator.Current.GetInstance<TransactionViewModel>(); }
        }

        #endregion DataAccess

        #region Views

        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        public SettingViewModel Setting
        {
            get { return ServiceLocator.Current.GetInstance<SettingViewModel>(); }
        }

        #endregion Views
    }
}