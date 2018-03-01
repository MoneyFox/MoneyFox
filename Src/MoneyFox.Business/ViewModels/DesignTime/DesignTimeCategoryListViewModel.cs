using System.Collections.ObjectModel;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation.Groups;
using MvvmCross.Core.ViewModels;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimeCategoryListViewModel : ICategoryListViewModel
    {
        public ObservableCollection<AlphaGroupListGroup<CategoryViewModel>> CategoryList =>
            new ObservableCollection<AlphaGroupListGroup<CategoryViewModel>>
            {
                new AlphaGroupListGroup<CategoryViewModel>("A")
                {
                    new CategoryViewModel(new Category()) {Name = "Auto"}
                },
                new AlphaGroupListGroup<CategoryViewModel>("E")
                {
                    new CategoryViewModel(new Category()) {Name = "Einkaufen"}
                }
            };

        public MvxAsyncCommand<CategoryViewModel> ItemClickCommand { get; }

        public CategoryViewModel SelectedCategory { get; set; }
        public string SearchText { get; set; }
    }
}
