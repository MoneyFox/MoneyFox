namespace MoneyFox.Ui.Controls.CategorySelection;

using CommunityToolkit.Mvvm.Input;
using Views;
using Views.Categories.CategorySelection;

public class CategorySelectionViewModel : BasePageViewModel
{
    private readonly INavigationService navigationService;

    private SelectedCategoryViewModel? selectedCategory;

    public CategorySelectionViewModel(INavigationService navigationService)
    {
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

    public AsyncRelayCommand GoToSelectCategoryDialogCommand => new(async () => await navigationService.OpenModalAsync<SelectCategoryPage>());

    public RelayCommand ResetCategoryCommand => new(() => SelectedCategory = null);

    protected override void OnDeactivated()
    {
        Messenger.UnregisterAll(this);
    }
}
