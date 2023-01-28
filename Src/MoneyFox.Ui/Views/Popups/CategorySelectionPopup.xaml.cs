namespace MoneyFox.Ui.Views.Popups;

using Categories;
using Resources.Strings;

public partial class CategorySelectionPopup
{
    public CategorySelectionPopup()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SelectCategoryViewModel>();
        var cancelItem = new ToolbarItem
        {
            Command = new Command(async () => await Navigation.PopModalAsync()),
            Text = Translations.CancelLabel,
            Priority = -1,
            Order = ToolbarItemOrder.Primary
        };

        ToolbarItems.Add(cancelItem);
    }

    private SelectCategoryViewModel ViewModel => (SelectCategoryViewModel)BindingContext;

    protected override async void OnAppearing()
    {
        await ViewModel.InitializeAsync();
    }
}
