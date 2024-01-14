namespace MoneyFox.Ui.Views.Categories.CategorySelection;

using Common.Navigation;
using Microsoft.Maui.Controls.PlatformConfiguration;
using Microsoft.Maui.Controls.PlatformConfiguration.iOSSpecific;

public partial class SelectCategoryPage : IBindablePage
{
    public SelectCategoryPage()
    {
        InitializeComponent();
        On<iOS>().SetModalPresentationStyle(UIModalPresentationStyle.FormSheet);
    }

    public SelectCategoryViewModel ViewModel => (SelectCategoryViewModel)BindingContext;
}
