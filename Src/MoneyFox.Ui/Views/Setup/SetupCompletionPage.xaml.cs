namespace MoneyFox.Ui.Views.Setup;

using Common.Navigation;

public partial class SetupCompletionPage: IBindablePage
{
    public SetupCompletionPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SetupCompletionViewModel>();
    }
}
