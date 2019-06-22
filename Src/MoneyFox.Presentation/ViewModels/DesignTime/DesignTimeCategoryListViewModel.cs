using System.Collections.ObjectModel;
using System.Globalization;
using GalaSoft.MvvmLight.Command;
using MoneyFox.Foundation.Groups;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.Utilities;

namespace MoneyFox.Presentation.ViewModels.DesignTime
{
    public class DesignTimeCategoryListViewModel : ICategoryListViewModel
    {
        public LocalizedResources Resources { get; } = new LocalizedResources(typeof(Strings), CultureInfo.CurrentUICulture);

        public ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> CategoryList =>
            new ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>>
            {
                new AlphaGroupListGroupCollection<CategoryViewModel>("A")
                {
                    new CategoryViewModel {Name = "Auto"}
                },
                new AlphaGroupListGroupCollection<CategoryViewModel>("E")
                {
                    new CategoryViewModel {Name = "Einkaufen"}
                }
            };

        public RelayCommand AppearingCommand { get; }

        public RelayCommand<CategoryViewModel> ItemClickCommand { get; }
        public RelayCommand<string> SearchCommand { get; }

        public CategoryViewModel SelectedCategory { get; set; }
        public string SearchText { get; set; }
        public bool IsCategoriesEmpty => false;
    }
}
