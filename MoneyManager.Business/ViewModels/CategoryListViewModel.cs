using System;
using System.Collections.ObjectModel;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;
using PropertyChanged;

namespace MoneyManager.Business.ViewModels {
    [ImplementPropertyChanged]
    public class CategoryListViewModel : ViewModelBase {
        private string _searchText;

        public CategoryListViewModel() {
            Categories = AllCategories;
        }

        public bool IsSettingCall { get; set; }
        public ObservableCollection<Category> Categories { get; set; }

        private IRepository<Category> CategoryRepository {
            get { return ServiceLocator.Current.GetInstance<IRepository<Category>>(); }
        }

        private ITransactionRepository TransactionRepository {
            get { return ServiceLocator.Current.GetInstance<ITransactionRepository>(); }
        }

        private ObservableCollection<Category> AllCategories {
            get { return CategoryRepository.Data; }
        }

        public Category SelectedCategory {
            get {
                return TransactionRepository.Selected == null
                    ? new Category()
                    : TransactionRepository.Selected.Category;
            }
            set {
                if (value == null) {
                    return;
                }

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
                    (AllCategories.Where(x => x.Name != null && x.Name.ToLower().Contains(_searchText.ToLower()))
                        .ToList());
            }
            else {
                Categories = AllCategories;
            }
        }
    }
}