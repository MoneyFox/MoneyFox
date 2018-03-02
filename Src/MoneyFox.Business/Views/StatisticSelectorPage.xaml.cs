using MoneyFox.Business.ViewModels.Statistic;
using MvvmCross.Forms.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Business.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class StatisticSelectorPage : MvxContentPage
    {
		public StatisticSelectorPage ()
		{
			InitializeComponent ();

		    StatisticSelectorList.ItemTapped += (sender, args) =>
		    {
		        StatisticSelectorList.SelectedItem = null;
                //((IStatisticSelectorViewModel) BindingContext).GoToStatisticCommand.Execute(args.Item);
		    };
		}
    }
}