namespace MoneyFox.Win.ViewModels.Categories;

using AutoMapper;
using CommunityToolkit.Mvvm.Messaging;
using Core._Pending_.Common.Messages;
using Core.Common.Interfaces;
using MediatR;
using Services;

internal class SelectCategoryListViewModel : AbstractCategoryListViewModel, ISelectCategoryListViewModel
{
    private CategoryViewModel? selectedCategory;

    /// <summary>
    ///     Creates an CategoryListViewModel for the usage of providing a CategoryViewModel selection.
    /// </summary>
    public SelectCategoryListViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService, INavigationService navigationService) : base(
        mediator: mediator,
        mapper: mapper,
        dialogService: dialogService,
        navigationService: navigationService) { }

    /// <summary>
    ///     CategoryViewModel currently selected in the view.
    /// </summary>
    public CategoryViewModel? SelectedCategory
    {
        get => selectedCategory;

        set
        {
            selectedCategory = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Post selected CategoryViewModel to message hub
    /// </summary>
    protected override void ItemClick(CategoryViewModel category)
    {
        Messenger.Send(new CategorySelectedMessage(category.Id));
    }
}
