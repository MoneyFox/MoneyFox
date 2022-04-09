namespace MoneyFox.Win.ViewModels.DesignTime;

using Categories;
using CommunityToolkit.Mvvm.Input;

public class DesignTimeModifyCategoryViewModel : IModifyCategoryViewModel
{
    public RelayCommand DeleteCommand { get; } = null!;

    public bool IsEdit { get; }
    public AsyncRelayCommand SaveCommand { get; } = null!;

    public AsyncRelayCommand CancelCommand { get; } = null!;

    public CategoryViewModel SelectedCategory { get; } = null!;
}
