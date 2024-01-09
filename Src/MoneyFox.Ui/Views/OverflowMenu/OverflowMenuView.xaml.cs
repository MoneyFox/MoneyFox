namespace MoneyFox.Ui.Views.OverflowMenu;

public partial class OverflowMenuView
{
    public OverflowMenuView()
    {
        InitializeComponent();
    }

    public OverflowMenuViewModel ViewModel => (OverflowMenuViewModel)BindingContext;
}
