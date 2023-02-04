namespace MoneyFox.Ui.Views.SetupAssistant;

public partial class CurrencyIntroductionPage
{
	public CurrencyIntroductionPage()
	{
		InitializeComponent();
        BindingContext = App.GetViewModel<CurrencyIntroductionViewModel>();
    }
}
