using System.Collections.ObjectModel;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Groups;
using MoneyFox.Foundation.Interfaces.ViewModels;

namespace MoneyFox.Shared.ViewModels.DesignTime
{
    public class DesignTimeCategoryListViewModel : ICategoryListViewModel
    {
        public DesignTimeCategoryListViewModel()
        {
            Categories = new ObservableCollection<CategoryViewModel>
            {
                new CategoryViewModel {Name = "Design Time CategoryViewModel 1"}
            };
        }

        public ObservableCollection<AlphaGroupListGroup<PaymentViewModel>> Source { get; set; }

        public ObservableCollection<CategoryViewModel> Categories { get; set; }
        public CategoryViewModel SelectedCategory { get; set; }
        public string SearchText { get; set; }
    }
}