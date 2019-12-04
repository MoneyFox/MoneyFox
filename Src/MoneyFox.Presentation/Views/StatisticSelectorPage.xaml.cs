using MoneyFox.Application.Resources;

namespace MoneyFox.Presentation.Views
{
    public partial class StatisticSelectorPage
    {
		public StatisticSelectorPage ()
		{
			InitializeComponent();
            BindingContext = ViewModelLocator.StatisticSelectorVm;

		    Title = Strings.StatisticsTitle;
		}
    }
}
