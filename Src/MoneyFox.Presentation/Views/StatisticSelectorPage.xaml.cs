using MoneyFox.Application.Resources;
using MoneyFox.Presentation.ViewModels.Statistic;

namespace MoneyFox.Presentation.Views
{
    public partial class StatisticSelectorPage
    {
        public StatisticSelectorPage()
        {
            InitializeComponent();
            BindingContext = ViewModelLocator.StatisticSelectorVm;

            StatisticSelectorList.ItemTapped += (sender, args) =>
            {
                StatisticSelectorList.SelectedItem = null;
                (BindingContext as StatisticSelectorViewModel)?.GoToStatisticCommand.Execute(args.Item);
            };

            Title = Strings.StatisticsTitle;
        }
    }
}
