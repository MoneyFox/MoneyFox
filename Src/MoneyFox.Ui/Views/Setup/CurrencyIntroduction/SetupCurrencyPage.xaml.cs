namespace MoneyFox.Ui.Views.Setup.CurrencyIntroduction;

public partial class CurrencyIntroductionPage
{
	public CurrencyIntroductionPage()
	{
		InitializeComponent();
        BindingContext = App.GetViewModel<SetupCurrencyViewModel>();
    }
}
