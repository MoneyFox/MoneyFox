namespace MoneyFox.Win.ViewModels.DesignTime;

using System.Collections.ObjectModel;
using Categories;
using CommunityToolkit.Mvvm.Input;
using Groups;

public class DesignTimeCategoryListViewModel : ICategoryListViewModel
{
    public CategoryViewModel SelectedCategory { get; set; } = null!;

    public string SearchText { get; set; } = "";

    public ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> CategoryList
        => new() { new("A") { new() { Name = "Auto" } }, new("E") { new() { Name = "Einkaufen" } } };

    public RelayCommand AppearingCommand { get; } = null!;

    public RelayCommand<CategoryViewModel> ItemClickCommand { get; } = null!;

    public AsyncRelayCommand<string> SearchCommand { get; } = null!;

    public bool IsCategoriesEmpty => false;
}
