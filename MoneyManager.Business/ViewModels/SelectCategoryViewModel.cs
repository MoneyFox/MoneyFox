using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    public class SelectCategoryViewModel : ViewModelBase
    {
        private string searchText;

        public SelectCategoryViewModel()
        {
            categoryData.LoadList();
            Categories = allCategories;
        }

        public ObservableCollection<Category> Categories { get; set; }

        private CategoryDataAccess categoryData
        {
            get { return ServiceLocator.Current.GetInstance<CategoryDataAccess>(); }
        }

        private ObservableCollection<Category> allCategories
        {
            get { return categoryData.AllCategories; }
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