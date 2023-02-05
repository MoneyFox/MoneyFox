namespace MoneyFox.Ui.Views.Setup;

public partial class SetupCompletionPage
{
    public SetupCompletionPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SetupCompletionViewModel>();
    }
}
