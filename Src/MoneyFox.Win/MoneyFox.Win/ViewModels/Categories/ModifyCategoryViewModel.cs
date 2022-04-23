namespace MoneyFox.Win.ViewModels.Categories;

using System.Threading.Tasks;
using AutoMapper;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core._Pending_.Common.Messages;
using Core.ApplicationCore.Queries;
using Core.Common.Interfaces;
using Core.Resources;
using MediatR;
using Services;
using DialogServiceClass = DialogService;

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
    protected ModifyCategoryViewModel(IMediator mediator, INavigationService navigationService, IMapper mapper, IDialogService dialogService)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        NavigationService = navigationService;
        DialogService = dialogService;
    }

    protected INavigationService NavigationService { get; }

    protected IDialogService DialogService { get; }

    public AsyncRelayCommand InitializeCommand => new(InitializeAsync);

    /// <summary>
    ///     Returns the Title based on whether a CategoryViewModel is being created or edited
    /// </summary>
    public string Title
    {
        get => title;

        set
        {
            if (title == value)
            {
                return;
            }

            title = value;
            OnPropertyChanged();
        }
    }

    public int CategoryId { get; set; }

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

    protected abstract Task InitializeAsync();

    protected abstract Task SaveCategoryAsync();

    private async Task SaveCategoryBaseAsync()
    {
        var openContentDialog = DialogServiceClass.GetOpenContentDialog();
        if (string.IsNullOrEmpty(SelectedCategory.Name))
        {
            DialogServiceClass.HideContentDialog(openContentDialog);
            await DialogService.ShowMessageAsync(title: Strings.MandatoryFieldEmptyTitle, message: Strings.NameRequiredMessage);
            await DialogServiceClass.ShowContentDialog(openContentDialog);

            return;
        }

        if (await mediator.Send(new GetIfCategoryWithNameExistsQuery(SelectedCategory.Name)))
        {
            DialogServiceClass.HideContentDialog(openContentDialog);
            await DialogService.ShowMessageAsync(title: Strings.DuplicatedNameTitle, message: Strings.DuplicateCategoryMessage);
            await DialogServiceClass.ShowContentDialog(openContentDialog);

            return;
        }

        await SaveCategoryAsync();
        Messenger.Send(new ReloadMessage());
    }

    private async Task CancelAsync()
    {
        SelectedCategory = mapper.Map<CategoryViewModel>(await mediator.Send(new GetCategoryByIdQuery(SelectedCategory.Id)));
    }
}
