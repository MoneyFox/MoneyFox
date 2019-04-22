using MoneyFox.Foundation.Resources;
using MvvmCross.Forms.Presenters.Attributes;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	[MvxTabbedPagePresentation(WrapInNavigationPage = false, Title = "Statistics", Icon = "ic_statistics_black")]
    public partial class StatisticSelectorPage
    {
		public StatisticSelectorPage ()
		{
			InitializeComponent();

		    StatisticSelectorList.ItemTapped += (sender, args) =>
		    {
		        StatisticSelectorList.SelectedItem = null;
                //ViewModel.GoToStatisticCommand.Execute(args.Item);
		    };

		    Title = Strings.StatisticsTitle;
		}
    }
}