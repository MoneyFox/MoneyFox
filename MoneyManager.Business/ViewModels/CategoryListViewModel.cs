#region

using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;

#endregion

namespace MoneyManager.Business.ViewModels {
    [ImplementPropertyChanged]
    public class CategoryListViewModel : ViewModelBase {
        private string _searchText;

        public CategoryListViewModel() {
            categoryData.LoadList();
            Categories = allCategories;
        }

        public bool IsSettingCall { get; set; }

        public ObservableCollection<Category> Categories { get; set; }

        private CategoryDataAccess categoryData {
            get { return ServiceLocator.Current.GetInstance<CategoryDataAccess>(); }
        }

        private ITransactionRepository TransactionRepository {
            get { return ServiceLocator.Current.GetInstance<ITransactionRepository>(); }
        }

        private ObservableCollection<Category> allCategories {
            get { return categoryData.AllCategories; }
        }

        public Category SelectedCategory {
            get {
                return TransactionRepository.Selected == null
                    ? new Category()
                    : TransactionRepository.Selected.Category;
            }
            set {
                if (value == null) return;

                if (!IsSettingCall) {
                    TransactionRepository.Selected.Category = value;
                    ((Frame) Window.Current.Content).GoBack();
                }
            }
        }

        public string SearchText {
            get { return _searchText; }
            set {
                _searchText = value;
                Search();
            }
        }

        public void Search() {
            if (SearchText != String.Empty) {
                Categories = new ObservableCollection<Category>
                    (allCategories.Where(x => x.Name != null && x.Name.ToLower().Contains(_searchText.ToLower()))
                        .ToList());
            } else {
                Categories = allCategories;
            }
        }
    }
}