namespace MoneyFox.Ui.Views.Popups;

using Categories;
using Categories.CategorySelection;
using Resources.Strings;

public partial class CategorySelectionPopup
{
    public CategorySelectionPopup()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<CategorySelectionViewModel>();
        var cancelItem = new ToolbarItem
        {
            Command = new Command(async () => await Navigation.PopModalAsync()),
            Text = Translations.CancelLabel,
            Priority = -1,
            Order = ToolbarItemOrder.Primary
        };

        ToolbarItems.Add(cancelItem);
    }

    private CategorySelectionViewModel SelectionViewModel => (CategorySelectionViewModel)BindingContext;

    protected override async void OnAppearing()
    {
        await SelectionViewModel.InitializeAsync();
    }
}
