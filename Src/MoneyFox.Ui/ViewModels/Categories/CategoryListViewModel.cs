namespace MoneyFox.Ui.ViewModels.Categories;

using System.Collections.ObjectModel;
using System.Globalization;
using AutoMapper;
using Common.Groups;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.ApplicationCore.Queries;
using Core.Commands.Categories.DeleteCategoryById;
using Core.Common.Interfaces;
using Core.Common.Messages;
using Core.Resources;
using MediatR;

internal class CategoryListViewModel : BaseViewModel
{
    private readonly IDialogService dialogService;
    private readonly IMapper mapper;

    private readonly IMediator mediator;

    private ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> categories = new();

    public CategoryListViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService)
    {
        this.mediator = mediator;
        this.mapper = mapper;
        this.dialogService = dialogService;
    }

    public ObservableCollection<AlphaGroupListGroupCollection<CategoryViewModel>> Categories
    {
        get => categories;

        private set
        {
            categories = value;
            OnPropertyChanged();
        }
    }

    public AsyncRelayCommand GoToAddCategoryCommand => new(async () => await Shell.Current.GoToAsync(Routes.AddCategoryRoute));

    public AsyncRelayCommand<string> SearchCategoryCommand => new(async searchTerm => await SearchCategoryAsync(searchTerm));

    public AsyncRelayCommand<CategoryViewModel> GoToEditCategoryCommand
        => new(async cvm => await Shell.Current.GoToAsync($"{Routes.EditCategoryRoute}?categoryId={cvm.Id}"));

    public AsyncRelayCommand<CategoryViewModel> DeleteCategoryCommand => new(async categoryViewModel => await DeleteAccountAsync(categoryViewModel));

    protected override void OnActivated()
    {
        Messenger.Register<CategoryListViewModel, ReloadMessage>(recipient: this, handler: (r, m) => r.SearchCategoryCommand.Execute(""));
    }

    protected override void OnDeactivated()
    {
        Messenger.Unregister<ReloadMessage>(this);
    }

    public async Task InitializeAsync()
    {
        await SearchCategoryAsync();
        IsActive = true;
    }

    private async Task SearchCategoryAsync(string searchTerm = "")
    {
        var categoryVms = mapper.Map<List<CategoryViewModel>>(await mediator.Send(new GetCategoryBySearchTermQuery(searchTerm)));
        var groups = AlphaGroupListGroupCollection<CategoryViewModel>.CreateGroups(
            items: categoryVms,
            ci: CultureInfo.CurrentUICulture,
            getKey: s => string.IsNullOrEmpty(s.Name) ? "-" : s.Name[0].ToString(CultureInfo.InvariantCulture).ToUpper(CultureInfo.InvariantCulture));

        Categories = new(groups);
    }

    private async Task DeleteAccountAsync(CategoryViewModel categoryViewModel)
    {
        if (await dialogService.ShowConfirmMessageAsync(
                title: Strings.DeleteTitle,
                message: Strings.DeleteCategoryConfirmationMessage,
                positiveButtonText: Strings.YesLabel,
                negativeButtonText: Strings.NoLabel))
        {
            await mediator.Send(new DeleteCategoryByIdCommand(categoryViewModel.Id));
            await SearchCategoryAsync();
        }
    }
}
