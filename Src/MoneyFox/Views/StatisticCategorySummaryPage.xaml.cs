using MoneyFox.Foundation.Resources;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StatisticCategorySummaryPage
	{
		public StatisticCategorySummaryPage ()
		{
            InitializeComponent ();
		    Title = Strings.CategorySummaryTitle;
		}
    }
}