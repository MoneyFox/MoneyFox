namespace MoneyFox.Ui.Views.Setup.SelectCurrency;

public partial class CurrencyIntroductionPage
{
    public CurrencyIntroductionPage()
    {
        InitializeComponent();
        BindingContext = App.GetViewModel<SetupCurrencyViewModel>();
    }
}
