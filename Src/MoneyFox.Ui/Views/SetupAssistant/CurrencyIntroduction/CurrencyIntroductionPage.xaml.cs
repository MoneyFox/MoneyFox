namespace MoneyFox.Ui.Views.SetupAssistant;

using MoneyFox.Ui.Views.SetupAssistant.CurrencyIntroduction;

public partial class CurrencyIntroductionPage
{
	public CurrencyIntroductionPage()
	{
		InitializeComponent();
        BindingContext = App.GetViewModel<CurrencyIntroductionViewModel>();
    }
}
