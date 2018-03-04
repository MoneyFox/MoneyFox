using MoneyFox.Business.Styles;
using MoneyFox.Foundation;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Platform;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Business.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StatisticCategorySummaryPage : ContentPage
	{
		public StatisticCategorySummaryPage ()
		{
		    ApplyRessources();
            InitializeComponent ();
		}

	    private void ApplyRessources()
	    {
	        if (Mvx.Resolve<ISettingsManager>().Theme == AppTheme.Dark)
	        {
	            Resources = new AppStylesDark();
	        }
	        else
	        {
	            Resources = new AppStylesLight();
	        }

	        Resources.MergedDictionaries.Add(new AppStyles());
	    }
    }
}