namespace MoneyFox.Views.OverflowMenu;

using Ui;
using ViewModels.OverflowMenu;

public partial class OverflowMenuPage
{
    public OverflowMenuPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<OverflowMenuViewModel>();
    }
}
