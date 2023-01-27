namespace MoneyFox.Ui.Views.Categories;

using System.Collections.ObjectModel;
using System.Globalization;
using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using MoneyFox.Core.ApplicationCore.Queries;
using MoneyFox.Core.Commands.Categories.DeleteCategoryById;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Core.Common.Messages;
using MoneyFox.Ui.Common.Groups;
using MoneyFox.Ui.Resources.Strings;
using ViewModels;

internal partial class CategoryListViewModel : BaseViewModel, IRecipient<ReloadMessage>
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
        => new(async cvm => await Shell.Current.GoToAsync($"{Routes.EditCategoryRoute}?categoryId={cvm.Id}"));

    public async void Receive(ReloadMessage message)
    {
        await SearchCategoryAsync();
    }

    public async Task InitializeAsync()
    {
        await SearchCategoryAsync();
        IsActive = true;
    }

    [RelayCommand]
    private async Task SearchCategoryAsync(string searchTerm = "")
    {
        var categoryVms = mapper.Map<List<CategoryListItemViewModel>>(await mediator.Send(new GetCategoryBySearchTermQuery(searchTerm)));
        var groups = AlphaGroupListGroupCollection<CategoryListItemViewModel>.CreateGroups(
            items: categoryVms,
            ci: CultureInfo.CurrentUICulture,
            getKey: s => string.IsNullOrEmpty(s.Name) ? "-" : s.Name[0].ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture));

        Categories = new(groups);
    }

    [RelayCommand]
    private async Task DeleteCategoryAsync(CategoryListItemViewModel categoryListItemViewModel)
    {
        if (await dialogService.ShowConfirmMessageAsync(
                title: Translations.DeleteTitle,
                message: Translations.DeleteCategoryConfirmationMessage,
                positiveButtonText: Translations.YesLabel,
                negativeButtonText: Translations.NoLabel))
        {
            await mediator.Send(new DeleteCategoryByIdCommand(categoryListItemViewModel.Id));
            await SearchCategoryAsync();
        }
    }
}
