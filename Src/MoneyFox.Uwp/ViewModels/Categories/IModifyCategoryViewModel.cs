using CommunityToolkit.Mvvm.Input;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Categories
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