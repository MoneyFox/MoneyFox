namespace MoneyFox.Win.ViewModels.Categories;

using AutoMapper;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Interfaces;
using Core.Common.Messages;
using MediatR;
using Services;

/// <inheritdoc cref="ISelectCategoryListViewModel" />
public class SelectCategoryListViewModel : AbstractCategoryListViewModel, ISelectCategoryListViewModel
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

    protected override void ItemClick(CategoryViewModel category)
    {
        var dataSet = new CategorySelectedDataSet(category.Id, category.Name);
        Messenger.Send(new CategorySelectedMessage(dataSet));
    }
}
