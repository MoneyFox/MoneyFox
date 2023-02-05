namespace MoneyFox.Ui.Views.Setup.SelectCurrency;

public partial class SetupCurrencyPage
{
    public SetupCurrencyPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SetupCurrencyViewModel>();
    }
}
