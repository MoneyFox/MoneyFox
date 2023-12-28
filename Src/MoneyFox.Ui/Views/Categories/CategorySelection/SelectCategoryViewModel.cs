namespace MoneyFox.Ui.Views.Categories.CategorySelection;

using System.Collections.ObjectModel;
using System.Globalization;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Core.Common.Interfaces;
using Core.Features.CategoryDeletion;
using Core.Queries;
using MediatR;
using ModifyCategory;
using Resources.Strings;

internal sealed class SelectCategoryViewModel(IDialogService service, ISender sender, INavigationService navigationService) : NavigableViewModel
{
    public const string SELECTED_CATEGORY_ID_PARAM = "selectedCategoryId";

    private ReadOnlyObservableCollection<CategoryGroup> categoryGroups = null!;

    public ReadOnlyObservableCollection<CategoryGroup> CategoryGroups
    {
        get => categoryGroups;
        private set => SetProperty(field: ref categoryGroups, newValue: value);
    }

    public AsyncRelayCommand GoToAddCategoryCommand => new(() => navigationService.GoTo<AddCategoryViewModel>());

    public AsyncRelayCommand<CategoryListItemViewModel> GoToEditCategoryCommand => new(cvm => navigationService.GoTo<EditCategoryViewModel>(cvm!.Id));

    public AsyncRelayCommand<string> SearchCategoryCommand => new(async s => await SearchCategoryAsync(s ?? string.Empty));

    public AsyncRelayCommand<CategoryListItemViewModel> DeleteCategoryCommand => new(async vm => await DeleteCategoryAsync(vm));

    public AsyncRelayCommand<CategoryListItemViewModel> SelectCategoryCommand => new(c => navigationService.GoBack(c!.Id));

    public override Task OnNavigatedAsync(object? parameter)
    {
        return SearchCategoryAsync();
    }

    public override Task OnNavigatedBackAsync(object? parameter)
    {
        return SearchCategoryAsync();
    }

    private async Task SearchCategoryAsync(string searchTerm = "")
    {
        var categories = await sender.Send(new GetCategoryBySearchTermQuery(searchTerm));
        var categoryVms = categories.Select(c => new CategoryListItemViewModel { Id = c.Id, Name = c.Name, RequireNote = c.RequireNote }).ToList();
        var groupedCategories = categoryVms.GroupBy(c => c.Name[0].ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture))
            .Select(g => new CategoryGroup(title: g.Key, categoryItems: g.ToList()));

        CategoryGroups = new(new(groupedCategories));
    }

    private async Task DeleteCategoryAsync(CategoryListItemViewModel? categoryListItemViewModel)
    {
        if (categoryListItemViewModel == null)
        {
            return;
        }

        if (await service.ShowConfirmMessageAsync(title: Translations.DeleteTitle, message: Translations.DeleteCategoryConfirmationMessage))
        {
            var numberOfAssignedPayments = await sender.Send(new GetNumberOfPaymentsAssignedToCategory.Query(categoryListItemViewModel.Id));
            if (numberOfAssignedPayments == 0
                || await service.ShowConfirmMessageAsync(
                    title: Translations.RemoveCategoryAssignmentTitle,
                    message: string.Format(format: Translations.RemoveCategoryAssignmentOnPaymentMessage, arg0: numberOfAssignedPayments),
                    positiveButtonText: Translations.RemoveLabel,
                    negativeButtonText: Translations.CancelLabel))
            {
                await sender.Send(new DeleteCategoryById.Command(categoryListItemViewModel.Id));
                await SearchCategoryAsync();
            }
        }
    }
}
