namespace MoneyFox.Ui.Views.Categories.ModifyCategory;

using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Interfaces;
using Core.Queries;
using MediatR;
using Resources.Strings;

public abstract class ModifyCategoryViewModel(IMediator mediator, IDialogService service) : NavigableViewModel
{
    private CategoryViewModel selectedCategory = null!;

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
            await service.ShowMessageAsync(title: Translations.MandatoryFieldEmptyTitle, message: Translations.NameRequiredMessage);

            return;
        }

        if (await mediator.Send(new GetIfCategoryWithNameExistsQuery(categoryName: SelectedCategory.Name, categoryId: SelectedCategory.Id)))
        {
            await service.ShowMessageAsync(title: Translations.DuplicatedNameTitle, message: Translations.DuplicateCategoryMessage);

            return;
        }

        try
        {
            await service.ShowLoadingDialogAsync(Translations.SavingCategoryMessage);
            await SaveCategoryAsync();
            await Shell.Current.Navigation.PopModalAsync();
        }
        finally
        {
            await service.HideLoadingDialogAsync();
        }
    }
}
