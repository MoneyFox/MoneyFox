using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess;
using MoneyManager.Models;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Linq;

namespace MoneyManager.ViewModels
{
    [ImplementPropertyChanged]
    public class SelectCategoryViewModel : ViewModelBase
    {
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

        public void Search(string keyword)
        {
            Categories = new ObservableCollection<Category>
                (allCategories.Where(x => x.Name.ToLower().Contains(keyword.ToLower())).ToList());
        }
    }
}