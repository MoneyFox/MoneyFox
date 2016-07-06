using System.Collections.ObjectModel;
using MoneyFox.Shared.Groups;
using MoneyFox.Shared.Interfaces.ViewModels;
using MoneyFox.Shared.Model;

namespace MoneyFox.Shared.ViewModels.DesignTime
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

        public ObservableCollection<AlphaGroupListGroup<Payment>> Source { get; set; }

        public ObservableCollection<Category> Categories { get; set; }
        public Category SelectedCategory { get; set; }
        public string SearchText { get; set; }
    }
}