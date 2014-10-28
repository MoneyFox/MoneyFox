using System.Collections.ObjectModel;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;

namespace MoneyManager.Business.ViewModels
{
    public class CategorySettingsViewModel : ViewModelBase
    {
        public CategorySettingsViewModel()
        {
            if (AllCategories == null)
            {
                categoryData.LoadList();
            }
        }

        private CategoryDataAccess categoryData
        {
            get { return ServiceLocator.Current.GetInstance<CategoryDataAccess>(); }
        }

        public ObservableCollection<Category> AllCategories
        {
            get { return categoryData.AllCategories; }
        }
    }
}
