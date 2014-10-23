using System.Collections.ObjectModel;
using System.Linq;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using PropertyChanged;

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    internal class SelectCategoryViewModel : ViewModelBase
    {
        private string searchText;

        public SelectCategoryViewModel()
        {
            Categories = allCategories;
        }

        public ObservableCollection<Category> Categories { get; set; }

        private ObservableCollection<Category> allCategories
        {
            get { return ServiceLocator.Current.GetInstance<CategoryDataAccess>().AllCategories; }
        }

        public Category SelectedCategory
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction.Category; }
            set { ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction.Category = value; }
        }

        public string SearchText
        {
            get { return searchText; }
            set
            {
                searchText = value;
                Search();
            }
        }

        public void Search()
        {
            Categories = new ObservableCollection<Category>
                (allCategories.Where(x => x.Name.ToLower().Contains(searchText.ToLower())).ToList());
        }
    }
}