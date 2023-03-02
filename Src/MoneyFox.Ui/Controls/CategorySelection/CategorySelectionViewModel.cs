namespace MoneyFox.Ui.Controls.CategorySelection;

using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using MoneyFox.Core.Interfaces;
using MoneyFox.Core.Queries;
using MoneyFox.Ui.Messages;
using MoneyFox.Ui.Views.Categories.CategorySelection;

internal class CategorySelectionViewModel : ObservableRecipient, IRecipient<CategorySelectedMessage>
{

    private readonly IMediator mediator;
    private readonly INavigationService navigationService;

    public CategorySelectionViewModel(IMediator mediator, INavigationService navigationService)
    {
        this.mediator = mediator;
        this.navigationService = navigationService;
    }

    private SelectedCategoryViewModel? selectedCategory;

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
}
