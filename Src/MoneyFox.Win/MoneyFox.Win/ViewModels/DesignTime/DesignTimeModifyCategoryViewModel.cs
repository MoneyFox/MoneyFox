namespace MoneyFox.Win.ViewModels.DesignTime;

using Categories;
using CommunityToolkit.Mvvm.Input;

public class DesignTimeModifyCategoryViewModel : IModifyCategoryViewModel
{
    public AsyncRelayCommand SaveCommand { get; } = null!;

    public AsyncRelayCommand CancelCommand { get; } = null!;

    public RelayCommand DeleteCommand { get; } = null!;

    public CategoryViewModel SelectedCategory { get; } = null!;

    public bool IsEdit { get; }
}