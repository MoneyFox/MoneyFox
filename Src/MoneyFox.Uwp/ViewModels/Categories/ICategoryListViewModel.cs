using CommunityToolkit.Mvvm.Input;
using MoneyFox.Uwp.Commands;
using MoneyFox.Uwp.Groups;
using System.Collections.ObjectModel;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Categories
{
    /// <summary>
    /// Defines the interface for a category list.
    /// </summary>
    public interface ICategoryListViewModel
    {
        /// <summary>
        /// List of categories.
        /// </summary>
        ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> CategoryList { get; }

        /// <summary>
        /// Command to handle when the view is appearing
        /// </summary>
        RelayCommand AppearingCommand { get; }

        /// <summary>
        /// Command for the item click.
        /// </summary>
        RelayCommand<CategoryViewModel> ItemClickCommand { get; }

        /// <summary>
        /// Search command
        /// </summary>
        AsyncCommand<string> SearchCommand { get; }

        /// <summary>
        /// Indicates if the category list is empty.
        /// </summary>
        bool IsCategoriesEmpty { get; }
    }
}