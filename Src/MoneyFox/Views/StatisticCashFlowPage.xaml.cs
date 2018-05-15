using MoneyFox.Foundation.Resources;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StatisticCashFlowPage
	{
		public StatisticCashFlowPage ()
		{
            InitializeComponent();
		    Title = Strings.CashFlowStatisticTitle;
		}
    }
}