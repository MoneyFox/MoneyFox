namespace MoneyFox.Win.ViewModels.Categories;

using AutoMapper;
using Core.Common.Interfaces;
using MediatR;
using Services;

internal sealed class CategoryListViewModel : AbstractCategoryListViewModel, ICategoryListViewModel
{
    /// <summary>
    ///     Creates an CategoryListViewModel for usage when the list including the option is needed.
    /// </summary>
    public CategoryListViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService, INavigationService navigationService) : base(
        mediator: mediator,
        mapper: mapper,
        dialogService: dialogService,
        navigationService: navigationService) { }

    /// <summary>
    ///     Post selected CategoryViewModel to message hub
    /// </summary>
    protected override void ItemClick(CategoryViewModel category)
    {
        EditCategoryCommand.Execute(category);
    }
}
