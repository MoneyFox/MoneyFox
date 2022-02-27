namespace MoneyFox.Win.ViewModels.Categories;

using Core._Pending_.Common.Interfaces;
using global::AutoMapper;
using MediatR;
using Services;

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
    protected override void ItemClick(CategoryViewModel category) => EditCategoryCommand.Execute(category);
}