using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Logic;
using MoneyManager.Business.Src;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using MoneyManager.Foundation;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    public class AddTransactionViewModel
    {
        #region Properties

        public FinancialTransaction SelectedTransaction
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction; }
            set { ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction = value; }
        }

        public ObservableCollection<Account> AllAccounts
        {
            get { return ServiceLocator.Current.GetInstance<AccountDataAccess>().AllAccounts; }
        }

        public ObservableCollection<Category> AllCategories
        {
            get { return ServiceLocator.Current.GetInstance<CategoryDataAccess>().AllCategories; }
        }

        public DateTime EndDate { get; set; }

        public bool IsEndless { get; set; }

        public bool IsEdit { get; set; }

        public int Recurrence { get; set; }

        public bool IsTransfer { get; set; }

        public bool RefreshRealtedList { get; set; }

        #endregion

        public string Title
        {
            get
            {
                string text = IsEdit
                    ? Translation.GetTranslation("EditTitle")
                    : Translation.GetTranslation("AddTitle");

                string type = TransactionTypeLogic.GetViewTitleForType(SelectedTransaction.Type);

                return String.Format(text, type);
            }
        }

        public void Save()
        {
            if (IsEdit)
            {
                TransactionLogic.UpdateTransaction(SelectedTransaction);
            }
            else
            {
                TransactionLogic.SaveTransaction(SelectedTransaction, RefreshRealtedList);
            }

            AccountLogic.AddTransactionAmount(SelectedTransaction);
            ((Frame)Window.Current.Content).GoBack();
        }

        public void Cancel()
        {
            if (IsEdit)
            {
                AccountLogic.AddTransactionAmount(SelectedTransaction);
            }

            ((Frame)Window.Current.Content).GoBack();
        }
    }
}