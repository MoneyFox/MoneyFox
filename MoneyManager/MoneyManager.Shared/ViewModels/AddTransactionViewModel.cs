using System;
using System.Collections.ObjectModel;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using MoneyManager.Src;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PropertyChanged;

namespace MoneyManager.ViewModels
{
    [ImplementPropertyChanged]
    public class AddTransactionViewModel
    {
        public bool IsEdit { get; set; }

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

        public string Title
        {
            get
            {
                var text = IsEdit
                    ? Utilities.GetTranslation("EditTitle")
                    : Utilities.GetTranslation("AddTitle");

                var type = TransactionTypeHelper.GetViewTitleForType(SelectedTransaction.Type);

                return String.Format(text, type);
            }
        }

        public DateTime EndDate { get; set; }

        public bool IsEndless { get; set; }

        public int Recurrence { get; set; }

        public void Save()
        {
            if (IsEdit)
            {
                ServiceLocator.Current.GetInstance<TransactionDataAccess>().Update(SelectedTransaction);
            }
            else
            {
                ServiceLocator.Current.GetInstance<TransactionDataAccess>().Save(SelectedTransaction);
            }

            ((Frame)Window.Current.Content).GoBack();
        }

        public void Cancel()
        {
            if (IsEdit)
            {
                ServiceLocator.Current.GetInstance<AccountDataAccess>().AddTransactionAmount(SelectedTransaction);
            }

            ((Frame)Window.Current.Content).GoBack();
        }
    }
}