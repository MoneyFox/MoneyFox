using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using PropertyChanged;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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

        private Category _localSelectedCategory;

        public Category LocalSelectedCategory
        {
            get { return _localSelectedCategory; }
            set
            {
                if (value == null) return;
                _localSelectedCategory = value;
                SelectedCategory = _localSelectedCategory;
                ((Frame)Window.Current.Content).GoBack();
            }
        }

        private Category SelectedCategory
        {
            set
            {
                ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction.Category = value;
            }
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
            if (SearchText != String.Empty)
            {
                Categories = new ObservableCollection<Category>
                    (allCategories.Where(x => x.Name.ToLower().Contains(searchText.ToLower())).ToList());
            }
            else
            {
                Categories = allCategories;
            }
        }
    }
}