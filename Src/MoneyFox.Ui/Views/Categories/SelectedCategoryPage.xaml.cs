namespace MoneyFox.Ui.Views.Categories;

using Core.Resources;
using ViewModels.Categories;

public partial class SelectCategoryPage : ContentPage
{
    public SelectCategoryPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SelectCategoryViewModel>();
        var cancelItem = new ToolbarItem
        {
            Command = new Command(async () => await Navigation.PopModalAsync()),
            Text = Strings.CancelLabel,
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

