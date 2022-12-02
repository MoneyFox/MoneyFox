namespace MoneyFox.Ui.ViewModels.Categories;

using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.ApplicationCore.Queries;
using Core.Common.Interfaces;
using Core.Common.Messages;
using Core.Resources;
using MediatR;

internal abstract class ModifyCategoryViewModel : BaseViewModel
{
    private readonly IDialogService dialogService;
    private readonly IMediator mediator;

    private CategoryViewModel selectedCategory = new();

    protected ModifyCategoryViewModel(IMediator mediator, IDialogService dialogService)
    {
        this.mediator = mediator;
        this.dialogService = dialogService;
    }

    public AsyncRelayCommand SaveCommand => new(async () => await SaveCategoryBaseAsync());

    public CategoryViewModel SelectedCategory
    {
        get => selectedCategory;

        set
        {
            selectedCategory = value;
            OnPropertyChanged();
        }
    }

    protected abstract Task SaveCategoryAsync();

    protected virtual async Task SaveCategoryBaseAsync()
    {
        if (string.IsNullOrEmpty(SelectedCategory.Name))
        {
            await dialogService.ShowMessageAsync(title: Strings.MandatoryFieldEmptyTitle, message: Strings.NameRequiredMessage);

            return;
        }

        if (await mediator.Send(new GetIfCategoryWithNameExistsQuery(SelectedCategory.Name)))
        {
            await dialogService.ShowMessageAsync(title: Strings.DuplicatedNameTitle, message: Strings.DuplicateCategoryMessage);

            return;
        }

        await dialogService.ShowLoadingDialogAsync(Strings.SavingCategoryMessage);
        await SaveCategoryAsync();
        Messenger.Send(new ReloadMessage());
        await dialogService.HideLoadingDialogAsync();
        await Shell.Current.Navigation.PopModalAsync();
    }
}


