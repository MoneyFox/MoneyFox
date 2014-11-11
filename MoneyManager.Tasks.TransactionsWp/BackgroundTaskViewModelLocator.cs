#region

using GalaSoft.MvvmLight.Ioc;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.ViewModels;
using MoneyManager.DataAccess.DataAccess;

#endregion

namespace MoneyManager.Tasks.TransactionsWp
{
    internal class BackgroundTaskViewModelLocator
    {
        static BackgroundTaskViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            SimpleIoc.Default.Register<AccountDataAccess>();
            SimpleIoc.Default.Register<TransactionDataAccess>();
            SimpleIoc.Default.Register<RecurringTransactionDataAccess>();

            SimpleIoc.Default.Register<TransactionListViewModel>();
        }

        #region DataAccess

        public AccountDataAccess AccountDataAccess
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>(); }
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

        public TransactionListViewModel TransactionListView
        {
            get { return ServiceLocator.Current.GetInstance<TransactionListViewModel>(); }
        }

        #endregion Views
    }
}