#region

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;
using PropertyChanged;

#endregion

namespace MoneyManager.Business.ViewModels
{
    [ImplementPropertyChanged]
    public class CategoryListViewModel : ViewModelBase
    {
        private string searchText;

        public CategoryListViewModel()
        {
            categoryData.LoadList();
            Categories = allCategories;
        }

        public bool IsSettingCall { get; set; }

        public ObservableCollection<Category> Categories { get; set; }

        private CategoryDataAccess categoryData
        {
            get { return ServiceLocator.Current.GetInstance<CategoryDataAccess>(); }
        }

        private TransactionDataAccess transactionData
        {
            get { return ServiceLocator.Current.GetInstance<TransactionDataAccess>();  }
        }

        private ObservableCollection<Category> allCategories
        {
            get { return categoryData.AllCategories; }
        }

        public Category SelectedCategory
        {
            get
            {
                return transactionData.SelectedTransaction == null
                    ? new Category()
                    : transactionData.SelectedTransaction.Category;
            }
            set
            {
                if (value == null) return;

                if (!IsSettingCall)
                {
                    ServiceLocator.Current.GetInstance<TransactionDataAccess>().SelectedTransaction.Category = value;
                    ((Frame) Window.Current.Content).GoBack();
                }
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