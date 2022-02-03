using CommunityToolkit.Mvvm.Input;
using MoneyFox.Win.Groups;
using System.Collections.ObjectModel;

namespace MoneyFox.Win.ViewModels.Categories
{
    /// <summary>
    ///     Defines the interface for a category list.
    /// </summary>
    public interface ICategoryListViewModel
    {
        /// <summary>
        ///     List of categories.
        /// </summary>
        ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> CategoryList { get; }

        /// <summary>
        ///     Command to handle when the view is appearing
        /// </summary>
        RelayCommand AppearingCommand { get; }

        /// <summary>
        ///     Command for the item click.
        /// </summary>
        RelayCommand<CategoryViewModel> ItemClickCommand { get; }

        /// <summary>
        ///     Search command
        /// </summary>
        AsyncRelayCommand<string> SearchCommand { get; }

        /// <summary>
        ///     Indicates if the category list is empty.
        /// </summary>
        bool IsCategoriesEmpty { get; }
    }
}