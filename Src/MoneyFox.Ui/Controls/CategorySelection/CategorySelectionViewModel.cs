namespace MoneyFox.Ui.Controls.CategorySelection;

using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Queries;
using MediatR;
using Views;
using Views.Categories.CategorySelection;

public class CategorySelectionViewModel : BasePageViewModel, IRecipient<CategorySelectedMessage>
{
    private readonly IMediator mediator;
    private readonly INavigationService navigationService;

    private SelectedCategoryViewModel? selectedCategory;

    public CategorySelectionViewModel(IMediator mediator, INavigationService navigationService)
    {
        this.mediator = mediator;
        this.navigationService = navigationService;
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

    public AsyncRelayCommand GoToSelectCategoryDialogCommand => new(async () => await navigationService.NavigateToAsync<SelectCategoryPage>());

    public RelayCommand ResetCategoryCommand => new(() => SelectedCategory = null);

    public void Receive(CategorySelectedMessage message)
    {
        var category = mediator.Send(new GetCategoryByIdQuery(message.Value.CategoryId)).GetAwaiter().GetResult();
        SelectedCategory = new() { Id = category.Id, Name = category.Name, RequireNote = category.RequireNote };
    }

    protected override void OnActivated()
    {
        Messenger.Register<CategorySelectionViewModel, SelectedCategoryRequestMessage>(recipient: this, handler: (r, m) => m.Reply(SelectedCategory?.Id));
    }
}
