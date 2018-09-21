using System.Collections.ObjectModel;
using System.Globalization;
using MoneyFox.Business.Helpers;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation.Groups;
using MoneyFox.Foundation.Resources;
using MvvmCross.Commands;

namespace MoneyFox.Business.ViewModels.DesignTime
{
    public class DesignTimeCategoryListViewModel : ICategoryListViewModel
    {
        public LocalizedResources Resources { get; } = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);

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
        public MvxAsyncCommand<string> SearchCommand { get; }

        public CategoryViewModel SelectedCategory { get; set; }
        public string SearchText { get; set; }
        public bool IsCategoriesEmpty => false;
    }
}
