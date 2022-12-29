namespace MoneyFox.Ui.Views.OverflowMenu;

public partial class OverflowMenuPage
{
    public OverflowMenuPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<OverflowMenuViewModel>();
    }
}
