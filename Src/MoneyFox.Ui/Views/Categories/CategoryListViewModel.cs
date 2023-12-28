namespace MoneyFox.Ui.Views.Categories;

using System.Collections.ObjectModel;
using System.Globalization;
using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Interfaces;
using Core.Features.CategoryDeletion;
using Core.Queries;
using MediatR;
using ModifyCategory;
using Resources.Strings;

public class CategoryListViewModel(IMediator mediator, IDialogService dialogService, INavigationService navigationService) : NavigableViewModel,
    IRecipient<CategoriesChangedMessage>
{
    private ReadOnlyObservableCollection<CategoryGroup> categoryGroups = null!;

    public ReadOnlyObservableCollection<CategoryGroup> CategoryGroups
    {
        get => categoryGroups;
        private set => SetProperty(field: ref categoryGroups, newValue: value);
    }

    public AsyncRelayCommand GoToAddCategoryCommand => new(() => navigationService.GoTo<AddCategoryViewModel>());

    public AsyncRelayCommand<CategoryListItemViewModel> GoToEditCategoryCommand => new(cvm => navigationService.GoTo<EditCategoryViewModel>(cvm!.Id));

    public AsyncRelayCommand<string> SearchCategoryCommand => new(async s => await SearchCategoryAsync(s ?? string.Empty));

    public AsyncRelayCommand<CategoryListItemViewModel> DeleteCategoryCommand => new(DeleteCategoryAsync);

    public void Receive(CategoriesChangedMessage message)
    {
        SearchCategoryAsync().GetAwaiter().GetResult();
    }

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
        var categories = await mediator.Send(new GetCategoryBySearchTermQuery(searchTerm));
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

        if (await dialogService.ShowConfirmMessageAsync(title: Translations.DeleteTitle, message: Translations.DeleteCategoryConfirmationMessage))
        {
            var numberOfAssignedPayments = await mediator.Send(new GetNumberOfPaymentsAssignedToCategory.Query(categoryListItemViewModel.Id));
            if (numberOfAssignedPayments == 0
                || await dialogService.ShowConfirmMessageAsync(
                    title: Translations.RemoveCategoryAssignmentTitle,
                    message: string.Format(format: Translations.RemoveCategoryAssignmentOnPaymentMessage, arg0: numberOfAssignedPayments),
                    positiveButtonText: Translations.RemoveLabel,
                    negativeButtonText: Translations.CancelLabel))
            {
                await mediator.Send(new DeleteCategoryById.Command(categoryListItemViewModel.Id));
                await SearchCategoryAsync();
            }
        }
    }
}
