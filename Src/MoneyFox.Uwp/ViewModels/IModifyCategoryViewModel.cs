using MoneyFox.Presentation.Commands;

namespace MoneyFox.Presentation.ViewModels
{
    public interface IModifyCategoryViewModel
    {
        /// <summary>
        ///     Saves changes to a CategoryViewModel
        /// </summary>
        AsyncCommand SaveCommand { get; }

        /// <summary>
        ///     Cancel the current operation
        /// </summary>
        AsyncCommand CancelCommand { get; }

        /// <summary>
        ///     Selected category.
        /// </summary>
        CategoryViewModel SelectedCategory { get; }
    }
}