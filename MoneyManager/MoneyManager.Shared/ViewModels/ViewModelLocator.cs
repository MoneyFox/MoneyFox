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
            SimpleIoc.Default.Register<AddAccountViewModel>();
            SimpleIoc.Default.Register<AddTransactionViewModel>();
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

        #endregion DataAccess

        #region Views

        public MainViewModel Main
        {
            get { return ServiceLocator.Current.GetInstance<MainViewModel>(); }
        }

        public AddAccountViewModel AddAccountViewModel
        {
            get { ServiceLocator.Current.GetInstance<AddAccountViewModel>(); }
        }

        public AddTransactionViewModel AddTransactionViewModel
        {
            get { return ServiceLocator.Current.GetInstance<AddTransactionViewModel>(); }
        }

        public SettingViewModel Setting
        {
            get { return ServiceLocator.Current.GetInstance<SettingViewModel>(); }
        }

        #endregion Views
    }
}