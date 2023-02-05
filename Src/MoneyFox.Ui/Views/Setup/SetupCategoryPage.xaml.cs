namespace MoneyFox.Ui.Views.Setup;

public partial class SetupCategoryPage
{
    public SetupCategoryPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SetupCategoryViewModel>();
    }
}
