using System.Collections.ObjectModel;
using MoneyFox.Business.ViewModels.Interfaces;
using MoneyFox.Foundation.Groups;
using MoneyFox.Service.Pocos;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimeCategoryListViewModel : ICategoryListViewModel
    {
        public DesignTimeCategoryListViewModel()
        {
            Categories = new ObservableCollection<CategoryViewModel>
            {
                new CategoryViewModel(new Category()) {Name = "Design Time CategoryViewModel 1"}
            };
        }

        public ObservableCollection<AlphaGroupListGroup<PaymentViewModel>> Source { get; set; }

        public ObservableCollection<CategoryViewModel> Categories { get; set; }
        public CategoryViewModel SelectedCategory { get; set; }
        public string SearchText { get; set; }
    }
}