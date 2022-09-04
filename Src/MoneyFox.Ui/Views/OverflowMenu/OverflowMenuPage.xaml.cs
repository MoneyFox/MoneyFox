namespace MoneyFox.Ui.Views.OverflowMenu;

using ViewModels.OverflowMenu;

public partial class OverflowMenuPage
{
    public OverflowMenuPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<OverflowMenuViewModel>();
    }
}
