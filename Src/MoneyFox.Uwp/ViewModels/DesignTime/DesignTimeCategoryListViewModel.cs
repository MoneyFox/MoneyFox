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

        public AsyncCommand AppearingCommand { get; }

        public RelayCommand<CategoryViewModel> ItemClickCommand { get; }

        public AsyncCommand<string> SearchCommand { get; }

        public CategoryViewModel SelectedCategory { get; set; }

        public string SearchText { get; set; }

        public bool IsCategoriesEmpty => false;
    }
}
