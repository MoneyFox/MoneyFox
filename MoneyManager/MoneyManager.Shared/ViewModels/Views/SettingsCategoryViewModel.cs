using GalaSoft.MvvmLight;
using MoneyManager.Models;
using MoneyManager.ViewModels.Data;
using System.Collections.ObjectModel;

namespace MoneyManager.ViewModels.Views
{
    public class SettingsCategoryViewModel : ViewModelBase
    {
        public ObservableCollection<Category> AllCategories
        {
            get { return new ViewModelLocator().CategoryViewModel.AllCategories; }
        }

        public CategoryViewModel CategoryViewModel
        {
            get { return new ViewModelLocator().CategoryViewModel; }
        }
    }
}