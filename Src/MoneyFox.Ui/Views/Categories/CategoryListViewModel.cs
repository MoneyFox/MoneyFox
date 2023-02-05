namespace MoneyFox.Ui.Views.Categories;

using System.Collections.ObjectModel;
using System.Globalization;
using AutoMapper;
using Common.Groups;
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
    private readonly IMapper mapper;

    private readonly IMediator mediator;

    private ObservableCollection<AlphaGroupListGroupCollection<CategoryListItemViewModel>> categories = new();

    public CategoryListViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.dialogService = dialogService;
    }

    public ObservableCollection<AlphaGroupListGroupCollection<CategoryListItemViewModel>> Categories
    {
        get => categories;

        private set
        {
            categories = value;
            OnPropertyChanged();
        }
    }

    public AsyncRelayCommand GoToAddCategoryCommand => new(async () => await Shell.Current.GoToAsync(Routes.AddCategoryRoute));

    public AsyncRelayCommand<CategoryListItemViewModel> GoToEditCategoryCommand
        => new(async cvm => await Shell.Current.GoToAsync($"{Routes.EditCategoryRoute}?categoryId={cvm?.Id}"));

    public AsyncRelayCommand<string> SearchCategoryCommand => new(async s => await SearchCategoryAsync(s ?? string.Empty));

    public AsyncRelayCommand<CategoryListItemViewModel> DeleteCategoryCommand => new(async vm => await DeleteCategoryAsync(vm));

    public async void Receive(CategoriesChangedMessage message)
    {
        await SearchCategoryAsync();
    }

    public async Task InitializeAsync()
    {
        await SearchCategoryAsync();
    }

    private async Task SearchCategoryAsync(string searchTerm = "")
    {
        var categoryVms = mapper.Map<List<CategoryListItemViewModel>>(await mediator.Send(new GetCategoryBySearchTermQuery(searchTerm)));
        var groups = AlphaGroupListGroupCollection<CategoryListItemViewModel>.CreateGroups(
            items: categoryVms,
            ci: CultureInfo.CurrentUICulture,
            getKey: s => string.IsNullOrEmpty(s.Name) ? "-" : s.Name[0].ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture));

        Categories = new(groups);
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
