namespace MoneyFox.Ui.Views.Categories.CategorySelection;

using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

public partial class SelectCategoryPage
{
    public SelectCategoryPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SelectCategoryViewModel>();
        On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.FormSheet);
    }

    private SelectCategoryViewModel ViewModel => (SelectCategoryViewModel)BindingContext;

    protected override void OnAppearing()
    {
        ViewModel.InitializeAsync().GetAwaiter().GetResult();
    }
}
