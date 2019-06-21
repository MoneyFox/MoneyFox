using MoneyFox.Foundation.Resources;
using MoneyFox.Presentation.ViewModels.Statistic;
using Xamarin.Forms.Xaml;

namespace MoneyFox.Presentation.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatisticSelectorPage
    {
		public StatisticSelectorPage ()
		{
			InitializeComponent();

		    StatisticSelectorList.ItemTapped += (sender, args) =>
		    {
		        StatisticSelectorList.SelectedItem = null;
                (BindingContext as StatisticSelectorViewModel)?.GoToStatisticCommand.Execute(args.Item);
		    };

		    Title = Strings.StatisticsTitle;
		}
    }
}