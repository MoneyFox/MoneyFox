using CommunityToolkit.Mvvm.Input;

namespace MoneyFox.Win.ViewModels.Categories
{
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
}