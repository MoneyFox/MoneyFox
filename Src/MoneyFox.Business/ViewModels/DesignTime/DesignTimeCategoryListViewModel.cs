using System.Collections.ObjectModel;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation.Groups;
using MvvmCross.Commands;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimeCategoryListViewModel : BaseViewModel, ICategoryListViewModel
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
        public MvxAsyncCommand LoadCategoriesCommand { get; }
        public MvxAsyncCommand CreateNewCategoryCommand { get; }
        public MvxAsyncCommand<string> SearchCommand { get; }

        public CategoryViewModel SelectedCategory { get; set; }
        public string SearchText { get; set; }
        public bool IsCategoriesEmpty => false;
    }
}
