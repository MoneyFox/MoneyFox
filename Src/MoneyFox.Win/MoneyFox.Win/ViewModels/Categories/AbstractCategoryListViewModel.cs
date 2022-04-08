namespace MoneyFox.Win.ViewModels.Categories;

using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core._Pending_.Common.Messages;
using Core.Commands.Categories.DeleteCategoryById;
using Core.Common.Interfaces;
using Core.Resources;
using Groups;
using MediatR;
using Pages.Categories;
using Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Core.Queries;

public abstract class AbstractCategoryListViewModel : ObservableRecipient
{
    private ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> source = new();

    protected AbstractCategoryListViewModel(IMediator mediator,
        IMapper mapper,
        IDialogService dialogService,
        INavigationService navigationService)
    {
        Mediator = mediator;
        Mapper = mapper;
        DialogService = dialogService;
        NavigationService = navigationService;
        IsActive = true;
    }

    protected INavigationService NavigationService { get; }

    protected IMediator Mediator { get; }

    protected IMapper Mapper { get; }

    protected IDialogService DialogService { get; }

    /// <summary>
    ///     Collection with categories alphanumeric grouped by
    /// </summary>
    public ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> CategoryList
    {
        get => source;
        private set
        {
            if(source == value)
            {
                return;
            }

            source = value;
            OnPropertyChanged();
            OnPropertyChanged(nameof(IsCategoriesEmpty));
        }
    }

    public bool IsCategoriesEmpty => !CategoryList?.Any() ?? true;

    public RelayCommand AppearingCommand => new(async () => await ViewAppearingAsync());

    /// <summary>
    ///     Deletes the passed CategoryViewModel after show a confirmation dialog.
    /// </summary>
    public AsyncRelayCommand<CategoryViewModel> DeleteCategoryCommand
        => new(DeleteCategoryAsync);

    /// <summary>
    ///     Edit the currently selected CategoryViewModel
    /// </summary>
    public RelayCommand<CategoryViewModel> EditCategoryCommand
        => new(
            async vm => await new EditCategoryDialog(vm.Id).ShowAsync());

    /// <summary>
    ///     Selects the clicked CategoryViewModel and sends it to the message hub.
    /// </summary>
    public RelayCommand<CategoryViewModel> ItemClickCommand => new(ItemClick);

    /// <summary>
    ///     Executes a search for the passed term and updates the displayed list.
    /// </summary>
    public AsyncRelayCommand<string> SearchCommand => new(SearchAsync);

    protected override void OnActivated() =>
        Messenger.Register<AbstractCategoryListViewModel, ReloadMessage>(
            this,
            (r, m) => r.SearchCommand.ExecuteAsync(""));

    protected override void OnDeactivated() => Messenger.Unregister<ReloadMessage>(this);

    /// <summary>
    ///     Handle the selection of a CategoryViewModel in the list
    /// </summary>
    protected abstract void ItemClick(CategoryViewModel category);

    public async Task ViewAppearingAsync() => await SearchAsync();

    /// <summary>
    ///     Performs a search with the text in the search text property
    /// </summary>
    public async Task SearchAsync(string searchText = "")
    {
        var categoriesVms =
            Mapper.Map<List<CategoryViewModel>>(await Mediator.Send(new GetCategoryBySearchTermQuery(searchText)));
        CategoryList = CreateGroup(categoriesVms);
    }

    private ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> CreateGroup(
        IEnumerable<CategoryViewModel> categories) =>
        new(
            AlphaGroupListGroupCollection<CategoryViewModel>.CreateGroups(
                categories,
                CultureInfo.CurrentUICulture,
                s => string.IsNullOrEmpty(s.Name)
                    ? "-"
                    : s.Name[0].ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture),
                itemClickCommand: ItemClickCommand));

    private async Task DeleteCategoryAsync(CategoryViewModel categoryToDelete)
    {
        if(await DialogService.ShowConfirmMessageAsync(
               Strings.DeleteTitle,
               Strings.DeleteCategoryConfirmationMessage))
        {
            await Mediator.Send(new DeleteCategoryByIdCommand(categoryToDelete.Id));
            await SearchAsync();
        }
    }
}