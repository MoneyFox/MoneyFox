namespace MoneyFox.Ui.Controls.CategorySelection;

using Common.Navigation;
using CommunityToolkit.Mvvm.Input;
using Views;
using Views.Categories.CategorySelection;

public class CategorySelectionViewModel(INavigationService navigationService) : NavigableViewModel
{
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

    public AsyncRelayCommand GoToSelectCategoryDialogCommand => new( () => navigationService.GoTo<CategorySelectionViewModel>());

    public RelayCommand ResetCategoryCommand => new(() => SelectedCategory = null);
}
