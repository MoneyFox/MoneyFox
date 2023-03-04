namespace MoneyFox.Ui.Controls.CategorySelection;

using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Queries;
using MediatR;
using Views;
using Views.Categories.CategorySelection;

public class CategorySelectionViewModel : BasePageViewModel
{
    private readonly IMediator mediator;
    private readonly INavigationService navigationService;

    private SelectedCategoryViewModel? selectedCategory;

    public CategorySelectionViewModel(IMediator mediator, INavigationService navigationService)
    {
        this.mediator = mediator;
        this.navigationService = navigationService;

        IsActive = true;
    }

    public SelectedCategoryViewModel? SelectedCategory
    {
        get => selectedCategory;

        set
        {
            selectedCategory = value;
            OnPropertyChanged();
        }
    }

    public AsyncRelayCommand GoToSelectCategoryDialogCommand => new(async () => await navigationService.OpenModalAsync<SelectCategoryPage>());

    public RelayCommand ResetCategoryCommand => new(() => SelectedCategory = null);

    protected override void OnActivated()
    {
        Messenger.Register<CategorySelectionViewModel, SelectedCategoryRequestMessage>(recipient: this, handler: (r, m) => m.Reply(SelectedCategory?.Id));
        Messenger.Register<CategorySelectionViewModel, CategorySelectedMessage>(recipient: this, handler: (r, m) => Receive(m));
    }

    protected override void OnDeactivated()
    {
        Messenger.UnregisterAll(this);
    }

    private void Receive(CategorySelectedMessage message)
    {
        var category = mediator.Send(new GetCategoryByIdQuery(message.Value)).GetAwaiter().GetResult();
        SelectedCategory = new() { Id = category.Id, Name = category.Name, RequireNote = category.RequireNote };
    }
}
