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
                return IsEdit
                    ? Utilities.GetTranslation("EditTitle")
                    : Utilities.GetTranslation("AddTitle");
            }
        }

        public void Save()
        {
            ServiceLocator.Current.GetInstance<TransactionDataAccess>().Save(SelectedTransaction);
            ((Frame)Window.Current.Content).GoBack();
        }

        public void Cancel()
        {
            ((Frame)Window.Current.Content).GoBack();
        }
    }
}