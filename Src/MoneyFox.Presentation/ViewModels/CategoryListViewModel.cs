using System.Collections.ObjectModel;
using AutoMapper;
using GalaSoft.MvvmLight.Command;
using MediatR;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.Groups;

namespace MoneyFox.Presentation.ViewModels
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
        AsyncCommand AppearingCommand { get; }

        /// <summary>
        ///     Command for the item click.
        /// </summary>
        RelayCommand<CategoryViewModel> ItemClickCommand { get; }

        /// <summary>
        ///     Search command
        /// </summary>
        AsyncCommand<string> SearchCommand { get; }

        /// <summary>
        ///     Indicates if the category list is empty.
        /// </summary>
        bool IsCategoriesEmpty { get; }
    }

    public class CategoryListViewModel : AbstractCategoryListViewModel, ICategoryListViewModel
    {
        /// <summary>
        ///     Creates an CategoryListViewModel for usage when the list including the option is needed.
        /// </summary>
        public CategoryListViewModel(IMediator mediator,
                                     IMapper mapper,
                                     IDialogService dialogService,
                                     INavigationService navigationService)
            : base(mediator, mapper, dialogService, navigationService)
        {
        }

        /// <summary>
        ///     Post selected CategoryViewModel to message hub
        /// </summary>
        protected override void ItemClick(CategoryViewModel category)
        {
            EditCategoryCommand.Execute(category);
        }
    }
}
