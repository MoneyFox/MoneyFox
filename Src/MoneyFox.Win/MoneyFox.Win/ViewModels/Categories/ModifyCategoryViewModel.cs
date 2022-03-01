using DialogServiceClass = MoneyFox.Win.DialogService;

namespace MoneyFox.Win.ViewModels.Categories;

using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core._Pending_.Common.Interfaces;
using Core._Pending_.Common.Messages;
using Core.Queries.Categories.GetCategoryById;
using Core.Queries.Categories.GetIfCategoryWithNameExists;
using Core.Resources;
using MediatR;
using Microsoft.UI.Xaml.Controls;
using Services;
using System.Threading.Tasks;

/// <summary>
///     View Model for creating and editing Categories without dialog
/// </summary>
public abstract class ModifyCategoryViewModel : ObservableRecipient, IModifyCategoryViewModel
{
    private readonly IMediator mediator;
    private readonly IMapper mapper;

    private CategoryViewModel selectedCategory = new();
    private string title = "";

    /// <summary>
    ///     Constructor
    /// </summary>
    protected ModifyCategoryViewModel(IMediator mediator,
        INavigationService navigationService,
        IMapper mapper,
        IDialogService dialogService)
    {
        this.mediator = mediator;
        this.mapper = mapper;

        NavigationService = navigationService;
        DialogService = dialogService;
    }

    protected abstract Task InitializeAsync();

    protected abstract Task SaveCategoryAsync();

    protected INavigationService NavigationService { get; }

    protected IDialogService DialogService { get; }

    public AsyncRelayCommand InitializeCommand => new(InitializeAsync);

    public AsyncRelayCommand SaveCommand => new(SaveCategoryBaseAsync);

    /// <summary>
    ///     Cancel the current operation
    /// </summary>
    public AsyncRelayCommand CancelCommand => new(CancelAsync);

    /// <summary>
    ///     The currently selected CategoryViewModel
    /// </summary>
    public CategoryViewModel SelectedCategory
    {
        get => selectedCategory;
        set
        {
            selectedCategory = value;
            OnPropertyChanged();
        }
    }

    /// <summary>
    ///     Returns the Title based on whether a CategoryViewModel is being created or edited
    /// </summary>
    public string Title
    {
        get => title;
        set
        {
            if(title == value)
            {
                return;
            }

            title = value;
            OnPropertyChanged();
        }
    }

    public int CategoryId { get; set; }

    private async Task SaveCategoryBaseAsync()
    {
        ContentDialog? openContentDialog = DialogServiceClass.GetOpenContentDialog();

        if(string.IsNullOrEmpty(SelectedCategory.Name))
        {
            DialogServiceClass.HideContentDialog(openContentDialog);
            await DialogService.ShowMessageAsync(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
            await DialogServiceClass.ShowContentDialog(openContentDialog);
            return;
        }

        if(await mediator.Send(new GetIfCategoryWithNameExistsQuery(SelectedCategory.Name)))
        {
            DialogServiceClass.HideContentDialog(openContentDialog);
            await DialogService.ShowMessageAsync(Strings.DuplicatedNameTitle, Strings.DuplicateCategoryMessage);
            await DialogServiceClass.ShowContentDialog(openContentDialog);
            return;
        }

        await SaveCategoryAsync();
        Messenger.Send(new ReloadMessage());
    }

    private async Task CancelAsync() => SelectedCategory =
        mapper.Map<CategoryViewModel>(await mediator.Send(new GetCategoryByIdQuery(SelectedCategory.Id)));
}