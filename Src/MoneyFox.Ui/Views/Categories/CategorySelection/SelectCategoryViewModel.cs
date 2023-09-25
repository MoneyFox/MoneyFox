namespace MoneyFox.Ui.Views.Categories.CategorySelection;

using System.Collections.ObjectModel;
using System.Globalization;
using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Interfaces;
using Core.Features.CategoryDeletion;
using Core.Queries;
using MediatR;
using Resources.Strings;

internal sealed class SelectCategoryViewModel : BasePageViewModel, IRecipient<CategoriesChangedMessage>
{
    public const string SELECTED_CATEGORY_ID_PARAM = "selectedCategoryId";
    private readonly IDialogService dialogService;
    private readonly IMediator mediator;
    private readonly INavigationService navigationService;

    private ReadOnlyObservableCollection<CategoryGroup> categoryGroups = null!;

    public SelectCategoryViewModel(IDialogService dialogService, IMapper mapper, IMediator mediator, INavigationService navigationService)
    {
        this.dialogService = dialogService;
        this.mediator = mediator;
        this.navigationService = navigationService;
    }

    public ReadOnlyObservableCollection<CategoryGroup> CategoryGroups
    {
        get => categoryGroups;
        private set => SetProperty(field: ref categoryGroups, newValue: value);
    }

    public AsyncRelayCommand GoToAddCategoryCommand => new(async () => await Shell.Current.GoToAsync(Routes.AddCategoryRoute));

    public AsyncRelayCommand<CategoryListItemViewModel> GoToEditCategoryCommand
        => new(async cvm => await Shell.Current.GoToAsync($"{Routes.EditCategoryRoute}?categoryId={cvm?.Id}"));

    public AsyncRelayCommand<string> SearchCategoryCommand => new(async s => await SearchCategoryAsync(s ?? string.Empty));

    public AsyncRelayCommand<CategoryListItemViewModel> DeleteCategoryCommand => new(async vm => await DeleteCategoryAsync(vm));

    public AsyncRelayCommand<CategoryListItemViewModel> SelectCategoryCommand
        => new(async c => { await navigationService.NavigateBackAsync(parameterName: SELECTED_CATEGORY_ID_PARAM, queryParameter: c.Id.ToString()); });

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
