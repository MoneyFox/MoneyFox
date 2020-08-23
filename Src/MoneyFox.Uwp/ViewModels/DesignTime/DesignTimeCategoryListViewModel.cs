using GalaSoft.MvvmLight.Command;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Ui.Shared.Groups;
using System.Collections.ObjectModel;

namespace MoneyFox.Uwp.ViewModels.DesignTime
{
    public class DesignTimeCategoryListViewModel : ICategoryListViewModel
    {
        public ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> CategoryList
                                                                                      => new ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>>
        {
            new AlphaGroupListGroupCollection<CategoryViewModel>("A")
            {
                new CategoryViewModel { Name = "Auto" }
            },
            new AlphaGroupListGroupCollection<CategoryViewModel>("E")
            {
                new CategoryViewModel { Name = "Einkaufen" }
            }
        };

        public AsyncCommand AppearingCommand { get; } = null!;

        public RelayCommand<CategoryViewModel> ItemClickCommand { get; } = null!;

        public AsyncCommand<string> SearchCommand { get; } = null!;

        public CategoryViewModel SelectedCategory { get; set; } = null!;

        public string SearchText { get; set; } = "";

        public bool IsCategoriesEmpty => false;
    }
}
