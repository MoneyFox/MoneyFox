using System.Collections.ObjectModel;
using MoneyFox.Foundation.Model;
using MoneyManager.Foundation.Interfaces.ViewModels;
using MoneyManager.Foundation.Model;

namespace MoneyManager.Core.ViewModels.DesignTime
{
    public class DesignTimeCategoryListViewModel : ICategoryListViewModel
    {
        public DesignTimeCategoryListViewModel()
        {
            Categories = new ObservableCollection<Category>
            {
                new Category {Name = "Design Time Category 1"}
            };
        }

        public ObservableCollection<Category> Categories { get; set; }
        public Category SelectedCategory { get; set; }
        public string SearchText { get; set; }
    }
}