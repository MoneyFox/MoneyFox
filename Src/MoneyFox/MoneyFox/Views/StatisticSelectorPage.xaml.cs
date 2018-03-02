using MoneyFox.Business.ViewModels.Statistic;
using MvvmCross.Forms.Views.Attributes;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	[MvxTabbedPagePresentation(WrapInNavigationPage = false)]
    public partial class StatisticSelectorPage
    {
		public StatisticSelectorPage ()
		{
			InitializeComponent ();

		    StatisticSelectorList.ItemTapped += (sender, args) =>
		    {
		        StatisticSelectorList.SelectedItem = null;
                ((IStatisticSelectorViewModel) BindingContext).GoToStatisticCommand.Execute(args.Item);
		    };
		}
    }
}