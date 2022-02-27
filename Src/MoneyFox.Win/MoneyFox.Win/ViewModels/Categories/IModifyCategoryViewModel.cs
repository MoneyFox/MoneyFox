namespace MoneyFox.Win.ViewModels.Categories;

using CommunityToolkit.Mvvm.Input;

public interface IModifyCategoryViewModel
{
    /// <summary>
    ///     Saves changes to a CategoryViewModel
    /// </summary>
    AsyncRelayCommand SaveCommand { get; }

    /// <summary>
    ///     Cancel the current operation
    /// </summary>
    AsyncRelayCommand CancelCommand { get; }

    /// <summary>
    ///     Selected category.
    /// </summary>
    CategoryViewModel? SelectedCategory { get; }
}