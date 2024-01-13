namespace MoneyFox.Ui.Views.Categories.ModifyCategory;

using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Interfaces;
using Core.Queries;
using MediatR;
using Resources.Strings;

public abstract class ModifyCategoryViewModel(IMediator mediator, IDialogService dialogService, INavigationService navigationService) : NavigableViewModel
{
    private CategoryViewModel selectedCategory = null!;

    public AsyncRelayCommand SaveCommand => new(async () => await SaveCategoryBaseAsync(), canExecute: () => SelectedCategory.IsValid);

    public CategoryViewModel SelectedCategory
    {
        get => selectedCategory;

        protected set
        {
            selectedCategory = value;
            OnPropertyChanged();
        }
    }

    protected abstract Task SaveCategoryAsync();

    protected async Task SaveCategoryBaseAsync()
    {
        if (await mediator.Send(new GetIfCategoryWithNameExistsQuery(categoryName: SelectedCategory.Name, categoryId: SelectedCategory.Id)))
        {
            await dialogService.ShowMessageAsync(title: Translations.DuplicatedNameTitle, message: Translations.DuplicateCategoryMessage);

            return;
        }

        try
        {
            await dialogService.ShowLoadingDialogAsync(Translations.SavingCategoryMessage);
            await SaveCategoryAsync();
            await navigationService.GoBack();
        }
        finally
        {
            await dialogService.HideLoadingDialogAsync();
        }
    }
}
