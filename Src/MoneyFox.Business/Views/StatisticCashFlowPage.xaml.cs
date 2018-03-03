using MoneyFox.Business.ViewModels.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Business.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StatisticCashFlowPage : ContentPage
	{
		public StatisticCashFlowPage ()
		{
            InitializeComponent ();
        }

	    protected override void OnAppearing()
	    {
	        base.OnAppearing();
	        ChartView.Chart = ((IStatisticCashFlowViewModel) BindingContext).Chart;
	    }
    }
}