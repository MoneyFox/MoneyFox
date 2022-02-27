namespace MoneyFox.Win.ViewModels.DesignTime;

using Categories;
using CommunityToolkit.Mvvm.Input;
using Groups;
using System.Collections.ObjectModel;

public class DesignTimeCategoryListViewModel : ICategoryListViewModel
{
    public ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> CategoryList
        => new ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>>
        {
            new("A") {new CategoryViewModel() {Name = "Auto"}},
            new("E") {new CategoryViewModel() {Name = "Einkaufen"}}
        };

    public RelayCommand AppearingCommand { get; } = null!;

    public RelayCommand<CategoryViewModel> ItemClickCommand { get; } = null!;

    public AsyncRelayCommand<string> SearchCommand { get; } = null!;

    public CategoryViewModel SelectedCategory { get; set; } = null!;

    public string SearchText { get; set; } = "";

    public bool IsCategoriesEmpty => false;
}