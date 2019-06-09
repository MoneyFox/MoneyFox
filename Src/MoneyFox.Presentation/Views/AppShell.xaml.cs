using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AppShell
    {
		public AppShell()
		{
            InitializeComponent();
        }

	    //protected override void OnChildAdded(Element child)
     //   {
	    //    if ((child as ContentPage)?.Title == "Accounts") {
	    //        ((ContentPage) child).Title  = Strings.AccountsTitle;
	    //        ((ContentPage) child).Icon  = StyleHelper.AccountImageSource as FileImageSource;
	    //    }
	    //    else if ((child as ContentPage)?.Title == "Statistics")
	    //    {
	    //        ((ContentPage)child).Title = Strings.StatisticsTitle;
	    //        ((ContentPage)child).Icon = StyleHelper.StatisticSelectorImageSource as FileImageSource;
	    //    }
     //       else if ((child as ContentPage)?.Title == "Settings") {
	    //        ((ContentPage) child).Title  = Strings.SettingsTitle;
	    //        ((ContentPage) child).Icon  = StyleHelper.SettingsImageSource as FileImageSource;
	    //    }

     //       base.OnChildAdded(child);
	    //}
	}
}