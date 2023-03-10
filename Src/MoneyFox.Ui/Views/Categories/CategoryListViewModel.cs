namespace MoneyFox.Ui.Views.Categories;

using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Interfaces;
using Core.Features.CategoryDeletion;
using Core.Queries;
using MediatR;
using Resources.Strings;

public class CategoryListViewModel : BasePageViewModel, IRecipient<CategoriesChangedMessage>
{
    private readonly IDialogService dialogService;
    private readonly IMediator mediator;

    public CategoryListViewModel(IMediator mediator, IDialogService dialogService)
    {
        this.mediator = mediator;
        this.dialogService = dialogService;
    }

    private ReadOnlyObservableCollection<CategoryGroup> categoryGroups = null!;
    public ReadOnlyObservableCollection<CategoryGroup> CategoryGroups
    {
        get => categoryGroups;
        private set => SetProperty(field: ref categoryGroups, newValue: value);
    }

    public AsyncRelayCommand GoToAddCategoryCommand => new(async () => await Shell.Current.GoToAsync(Routes.AddCategoryRoute));

    public AsyncRelayCommand<CategoryListItemViewModel> GoToEditCategoryCommand
        => new(async cvm => await Shell.Current.GoToAsync($"{Routes.EditCategoryRoute}?categoryId={cvm?.Id}"));

    public AsyncRelayCommand<string> SearchCategoryCommand => new(async s => await SearchCategoryAsync(s ?? string.Empty));

    public AsyncRelayCommand<CategoryListItemViewModel> DeleteCategoryCommand => new(DeleteCategoryAsync);

    public void Receive(CategoriesChangedMessage message)
    {
        SearchCategoryAsync().GetAwaiter().GetResult();
    }

    public async Task InitializeAsync()
    {
        await SearchCategoryAsync();
    }

    private async Task SearchCategoryAsync(string searchTerm = "")
    {
        var categories = await mediator.Send(new GetCategoryBySearchTermQuery(searchTerm));
        var categoryVms = categories.Select(c => new CategoryListItemViewModel
        {
            Id = c.Id,
            Name = c.Name,
            RequireNote = c.RequireNote,
        }).ToList();

        var groupedCategories = categoryVms.GroupBy(c => c.Name[0].ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture))
            .Select(g => new CategoryGroup(g.Key, g.ToList()));
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
